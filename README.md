# LaBuilderApp

L'application pour l'association [R2-Builders](http://www.r2builders.fr).

![logo](http://www.r2builders.fr/images/nouvellebanniere1024.png)

Cette application est un prolongement du site web [www.r2builders.fr](http://www.r2builders.fr)   

Fonctionnalités
---------------
- visualiser les sorties du club  
- consulter la liste des Builders  
- voir des vidéos du club et des robots  

Développement
-------------
Utilisation de la plateforme de développement Xamarin Forms ([5]). L'application est disponible en version iOS et Android.  
Du code php a été ajouté au backoffice phpbb, moteur du site web de l'association, permettant l'intégration de l'application avec le site web.  

Infos utilisées
---------------
Les codes pays sont les codes iso utilisés par exemple en téléphonie.  
Les images de drapeau proviennent de ce site : [http://flagpedia.net/](http://flagpedia.net/).  

Packages utilisés
-----------------
Device Info, to display informations of the runing device ([1]).  
Media, to take pictures ([2]).  
App info, to display informations of the current app ([3]).  
Settings, to store settings on the device ([4]).  

Remerciements à
---------------
https://gist.github.com/NicoVermeir/7ffb34ebd639ed958382 pour le Awesome Wrap Panel  
https://xamarinhelp.com/carousel-view-page-indicators/  pour l'ajout d'indicateur sur le carousel  
http://www.flaticon.com : Icon made by Freepik, Pixel Buddha, Madebyoliver, Lyolya from www.flaticon.com  

à voir  
------
https://github.com/XAM-Consulting/Xam.Plugins.ImageCropper : pas compatible avec version de PCL :(  
https://github.com/luberda-molinet/FFImageLoading  : bug sous iOS au moment ou j'ai essayé
https://github.com/LosXamarinos/Xamarin-Forms-Sound-Player  


[1]: https://github.com/jamesmontemagno/DeviceInfoPlugin
[2]: https://github.com/jamesmontemagno/MediaPlugin
[3]: https://github.com/Aftnet/AppInfoPlugin
[4]: https://github.com/jamesmontemagno/SettingsPlugin
[5]: https://www.xamarin.com

note perso:  
missing icons  
iphone/ipod  
app icon 120x120 en .png  
ipad  
app icon 76x76 en .png  
app icon 167x167 en .png  
app icon 152x152 en .png  
