# Exercice 1

Les régularisations sont une sous-partie des règles possibles du nouveau moteur de règle de Figgo. L'idée est qu'un admin puisse créer plein de règles qui, en fonction d'un contexte de données, vont pouvoir générer des écritures.

Par exemple, si on a crédité 2 jours de Congés Payés sur le mois de février 2021 (4 semaines de 5 jours) et qu'un utilisateur était en Arrêt Maladie pendant 5 jours, on aimerait lui retirer 0.5 jours, c'est-à-dire lui retirer des congés au prorata de son temps d'absence.

Détail du calcul :
- Nombre de jours de travail sur le mois : 4 semaines de 5 jours = 20 jours
- Nombre de jours d'absence d'arrêt maladie : 5 jours
- Ratio d'absence : 5/20 = 0.25
- Débit issu de la régul : ratio * nombre_de_jours_crédités = 0.25 * 2 = 0.5

Dans ce cas, pour cette règle et cet utilisateur, une écriture avec 0.5 de débit sera créée.

Le but de l'exercice est de pouvoir modéliser le plus simplement possible la règle de régularisation, mais surtout de créer un builder permettant de faciliter la création de règle de régul, notamment pour les tests unitaires où les règles sont quasiment toutes paramétrées de la même façons, à quelques détails près.

## Étape 1 : La base

L'objectif est de modéliser les objets métier qui suivent. Pas de sur-ingéniérie, faites au plus simple, quitte à faire des modèles anémiques.

Un compte permet de définir le type d'absence, il est composé de
  - un numéro (ex: 1020)
  - un nom (ex: Congés payés 2020)

Une règle de régularisation est composée de plusieurs choses :
- un id (int)
- un ou plusieurs comptes impactants qui permettent de récupérer les absences sur lesquelles se basera le calcul (ex : Compte maladie)
- un compte cible, qui représente le compte de l'écriture générée (ex : écriture de -0.5 sur le compte Congés Payés)
- une population qui est composée de
  - plusieurs id d'entités légales
  - plusieurs id de départements
  - plusieurs id de profils d'acquisition


Une fois les modèles faits, on aimerait construire un builder pour simplifier la création des règuls pour les tests. La majorité des tests ont besoin de la même règle de régul :
- Id: 1
- Comptes impactants : Maladie
- Compte cible : Congés payés 2020
- Population :
  - Profils : 1
  - Entités légales : rien
  - Département : rien

Cependant certains tests ont besoin de renseigner d'autres comptes, par exemple :
- Comptes impactants : Télétravail, Formation interne
- Compte cible : RTT

Rajouter la possibilité de changer les comptes au sein du Builder.


## Étape 2 : Fluent

Tout ça marche bien mais on peut écrire n'importe quoi, du genre :
```csharp
var regul =
    new RegulBuilder()
        .ComptesImpactants(Comptes.Maladie, Comptes.Teletravail)
        .CompteCible(Comptes.Cp2020)
        .CompteCible(Comptes.Cp2021)
        .CompteCible(Comptes.Rtt)
        .ComptesImpactants(Comptes.FormationInterne)
        .Build();
```

Convertir le builder actuel en Builder Fluent pour pouvoir guider la construction :
1. Instanciation du Builder
2. Comptes impactants (valeurs par défaut si non renseigné)
3. Compte cible (valeur par défaut si non renseigné)
4. Build

S'assurer qu'on puisse appeler Build à chaque étape.


## Étape 3 : Population

Maintenant, c'est la population qu'on aimerait pouvoir paramétrer dans le builder.

On pourrait très bien faire une méthode comme ce code :
```csharp
RegulBuilder Population(Population population)
```

... mais ça oblige dans les tests à instancier la population et ça peut vite rendre le code verbeux :

```csharp
var regul =
    new RegulBuilder()
        .ComptesImpactants(Comptes.Maladie, Comptes.Teletravail)
        .CompteCible(Comptes.Rtt)
        .Population(new Population {
            LegalEntityIds = new [] { 1 },
            ProfileIds = new [] { 2, 3 }
        })
        .Build();
```

Même remarque avec les paramètres optionnels même si c'est un poil moins verbeux

```csharp
RegulBuilder Population(
    IReadOnlyCollection<int> legalEntityIds = null,
    IReadOnlyCollection<int> departementIds = null,
    IReadOnlyCollection<int> profilesIds = null
);


var regul =
    new RegulBuilder()
        .ComptesImpactants(Comptes.Maladie, Comptes.Teletravail)
        .CompteCible(Comptes.Rtt)
        .Population(
            legalEntityIds: new [] { 1 },
            profileIds = new [] { 2, 3 }
        })
        .Build();
```

Mais en dehors de la verbosité, on a un plus gros problème dans les tests. En effet, on se rend compte que la valeur du profil change quasiment pour chaque test et donc pas moyen d'en sortir une valeur par défaut pertinente. Autre point important, les règles métier ont un peu changé et il faut maintenant au moins un profil. Les départements et entités légales peuvent ne pas être renseignées.

### Instruction 1

Créer un builder fluent pour la population obligeant à saisir au moins un profil.


### Instruction 2

Utiliser le builder de population dans le builder principal en s'assurant que le Build du builder de population est bien appelé lors du Build du builder principal.


### Instruction 3 (optionnel)

Nouvelle demande, on a besoin de l'identifiant du réglementaire dans la règle de régul. Cet identifiant de réglementaire est obligatoire et doit être renseigné au tout début du builder. Le rajouter dans le builder, cette étape est obligatoire et il n'y a pas de valeur par défaut.

Ce réglementaire est lié à certaines entités légales et le code utilisant nos règles de régul a besoin de vérifier si les entités légales de la population sont bien comprises dans le réglementaire. Ce qui veut dire que la population doit aussi avoir cet identifiant de réglementaire renseigné. Le rajouter dans le builder de population, cette étape est obligatoire et il n'y a pas de valeur par défaut.

S'assurer qu'on ne puisse pas renseigner un id de réglementaire dans le builder de population qui soit différent de celui renseigné par le builder principal. Essayez de résoudre cette contrainte non pas avec une exception, mais juste par design.


## Étape 4 : Seuil et plafond

Un nouveau besoin est demandé par les clients, le fait de pouvoir mettre un seuil et un plafond pour les réguls.

En effet, certains clients aimeraient que le décompte des jours ne se fasse qu'après un certain nombre de jours d'absence, c'est le seuil.

Aussi, pour éviter de se faire sucrer tous leurs congés à cause d'une maladie longue durée, certains clients aimeraient avoir un plafond au-delà duquel la règul arrêtera de retirer des jours, c'est le plafond.

Le calcul du seuil et du plafond dépend de deux autres paramètres :
- La consécutivité (ou pas) des jours impactant
- Le mode de calcul de la période de seuil
  - Sur la période de calcul
  - Depuis le début de l'acquisition
  - Depuis les 12 derniers mois

Le seuil et le plafond sont complètement facultatifs. Cependant, si l'un des deux est renseigné il faut obligatoirement savoir s'ils sont consécutifs ou pas. Le mode de calcul est par défaut sur la période de calcul, mais il faut aussi pouvoir le changer.

### Explications métier (pour les passionné·e·s de calculs tordus)

Pour reprendre l'exemple du tout début de l'exercice : si on a crédité 2 jours de Congés Payés sur le mois de février 2021 (4 semaines de 5 jours) et qu'un utilisateur était en Arrêt Maladie consécutif pendant 35 jours (20 jours en janvier et 15 en février), on aimerait lui retirer des jours au prorata à partir du 30e jour d'absence consécutif.

Détail du calcul (pour les passionné·e·s de calculs tordus) :
- Nombre de jours de travail sur le mois : 4 semaines de 5 jours = 20 jours
- Nombre de jours de travail à prendre en compte : 10 jours
  - Seuil atteint le 10e jour ouvré de février car on a 20 jours en janvier, il reste donc 10 (30-20) jours avant d'atteindre le seuil
  - Nombre de jours à vraiment considerer sur le mois = nombre_jours_mois - reste_jours_seuil = 20 - 10 = 10
- Nombre de jours d'absence d'arrêt maladie au delà du seuil sur le mois : 5 jours
- Ratio d'absence : jours_absence / nombre_de_jours_a_considerer = 5 / 10 = 0.5
- Débit issu de la régul : ratio * nombre_de_jours_crédités = 0.5 * 2 = 1

Avec un seuil à 30, la régul génèrera donc une écriture avec 1 de débit

### Instructions

Ajouter les champs liés au seuil/plafond dans l'objet de Regul.

Gérer les champs liés au seuil/plafond dans le builder de Regul.


## Étape 5 : Catégories de compte cible

Il existe deux grands types de comptes dans Figgo, les comptes simples (ex: Maladie, Télétravail...) et les comptes millésimés (ex: Congés payés). Un compte millésimé est constitué d'une catégorie de compte qui englobera tous les comptes qui lui sont rattachés.

Par exemple, on a une catégorie de compte « Congés payés » à laquelle sont rattachés les comptes suivants :
- CP 2020/2021
- CP 2021/2022
- CP 2022/2023
- ...

Une catégorie de comptes est constitué de
- un identifiant
- un nom
- la liste des comptes qui lui sont rattachés

Il faut maintenant pouvoir renseigner en tant que compte cible :
- soit un compte simple
- soit un compte millésimé

Ajouter les champs nécessaires dans les modèles.

Ajouter la possibilité de renseigner le compte millésimé dans le builder. On doit pouvoir renseigner soit un compte soit un compte millésimé mais pas les deux en même temps.