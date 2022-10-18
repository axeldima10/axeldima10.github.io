---
title: L'injection de dépendance en C# (.NET 6)
date: 2022-10-14
description: "L'injection de dépendance est une bonne pratique pour réduire la dépendance entre les objets, augmenter la testabilité de votre code et rendre votre application plus facile à mettre à niveau."
---

L'injection de dépendance nous permet de déléguer la création et la gestion de vie d'une instance de classe en sorte que cette instance soir injectée lorsque nous en avons besoin. 

L'injection de dépendance est en fait une application de *[l'inversion de dépendance](/fr/posts/inversion-dependance)*