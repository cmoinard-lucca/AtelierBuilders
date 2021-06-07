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
