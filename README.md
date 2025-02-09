# Connection string
Ma connection string provient de l'emulateur cosmos db local.
J'ai creer une db dedans et j'utilise le user et le port provided par l'interface.

# Ce que j'ai fais
- J'ai adapte programme.cs pour ne plus utiliser de migration. Il enregistre maintenant le dbcontext au demarage.
- Les models on ete retravailler. Ils n'ont plus de foreign key et leur primary sont des strings (mieux supporter par cosmosdb)
- DbContext j'ai fais quelque update pour que ca marche correctement avec cosmosdb
- J'ai ameliorer la view de post pour quelle ne throw pas derreur quand on l'essaye. C'etait du au fais quelle fesait un select sql. Maintenant elle:
    - va chercher le post.
    - va chercher les comments du post.
    - build la view en fonction.
    - Renvoye les deux ensemble
    