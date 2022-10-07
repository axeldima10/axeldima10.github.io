---
title: "Installer WSL2 sur Linux"
date: 2022-09-28
description: "Pour les utilisateurs de Windows, il est possible d'utiliser les distributions populaires de Linux de manière presque native grâce au Windows Substystem for Linux. Dans cet article je me propose de vous présenter les intérêts d'avoir une telle configuration sur votre PC et je vous dans l'installation de WSL2 sur un PC Windows 11."
image: /blog/windows-ubuntu.png
---

# Le sous-système pour Linux

## Qu'est-ce ?

Le sous système Linux pour Windows est une couche de compabilité développée par Microsoft qui permet faire fonctionner les binaires exécutables du système d'exploitation Linux de manière native sur Windows 10, 11 et Windows Server 2019 et ultérieur.
La première version beta de WSL a été introduite en Août 2016 sur Windows 10. A cette époque seule Ubuntu (avec Bash par défaut) était supporté.  

Ce n'est pas la première fois que Microsoft développe une solution pour apporter la compatibilité de Linux à son système d'exploitation. Parmi les précedentes tentatives de Microsoft pour faire fonctionner Linux sur Windows nous pouvons mentionner Microsoft POSIX, Windows Services for UNIX (SFU), Cygwin. 

WSL se propose comme un moyen sérieux et durable pour faire fonctionner nativement les exécutables Linux sur Windows. Vu le travail que Microsoft consacre sur ce projet nous pouvons espérer que WSL soit un outil pérenne. 


# Installation de WSL2 sur Windows 11

WSL est supporté à la fois sur la version Famille et la version professionnelle de Windows 11. 


## Approche graphique 
Cette approche est la plus lente mais convient bien si vous êtes plus visuel. 
La manipulation est simple, il vous suffit d'accéder à la fenêtre **Fonctionnalité de Windows**. Rien de plus simple, tapez "*Activer ou désactiver les fonctionnalités Windows*". 

Vous devez trouver quelque chose du genre sur votre ordinateur. 
![activer fonctionnalités Windows](/blog/wls2/activer-fonctionnalite.png)

Vous cliquer sur l'icône pour accéder la fenêtre ci-dessous où vous pouvez activer le Sous-système Windows pour Linux. 

![activer WSL](/blog/wls2/activer-wsl.png)

Vous cochez la case Sous-système pour Linux et c'est fait. Windows vous demandera de redémarrer votre ordinateur pour que l'activation soit effective. 
Une fois votre ordinateur redémarré vous voilà avec WSL 2 prêt à l'emploi !

## Approche en ligne de commande 

Une autre façon d'activer WSL est de passer par la ligne de commande. 
Cette solution peut est très rapide si vous êtes habitué à utiliser CMD ou Powershell. 

Pour le faire, ouvrez un terminal avec les privilèges administrateur. 

![Ouvrir Terminal](/blog/wls2/ouvrir-terminal.png)

Une fois le Terminal ouvert, tapez la commande suivante. 

```cmd
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
```

Il vous faudra toujours redémarrer votre ordinateur pour que l'activation soit effective. Le paramètre `/norestart` dans la commande précédente empêche Windows de forcer un redémarrage mais vous devez le faire avant d'installer une distribution Linux.

# Installer une distribution Linux

La plupart des personnes que je connais qui utilisent WSL ont tendance à installer Ubuntu comme système d'exploitation. Il n'est pas obligatoire d'installer Ubuntu. Il existe une liste (restreinte) de distributions que vous pouvez installer parmi lesquels Debian et Kali Linux.  Installez la distribution qui vous semble plus familière. Vous pouvez même installer plusieurs distributions sur votre ordinateur et les utiliser simultanément. 

Pour lister les distributions disponibles vous pouvez utiliser la ligne de commande. 

```cmd
wsl --list --online
```
Pour installer une de ces distribution vous tapez **« wsl --install -d <Distribution> »**. 
Par exemple pour installer Ubuntu 02-04 LTS faites

```cmd
wsl --install -d Ubuntu-20-04
```

Si vous savez déjà quelle distribution vous voulez installer, vous pouvez aussi l'installer via le Windows Store en recherchant le nom de la distribution dans la barre de recherche. 

![](/blog/wls2/debian.png)



# Quel intérêt ?

Vous pouvez vous demander quel est l'intérêt d'avoir une distribution Linux sur votre système Windows ? Je vous dirai l'interopérabilité ! La possibilité d'utiliser à la fois l'environnement graphique de Windows tout en exploitant la capacité de scripting de Linux peut être une très bonne combinaison à avoir. 

Si vous êtes un habitué de Linux et que Windows ne vous dit rien alors je pense que WSL ne va pas vous apporter grand chose. 

# Sources
Windows SubSystem for Linux, https://en.wikipedia.org/wiki/Windows_Subsystem_for_Linux#cite_note-3

How to install WSL2 on Windows 10/11, https://cloudbytes.dev/snippets/how-to-install-wsl2-on-windows-1011