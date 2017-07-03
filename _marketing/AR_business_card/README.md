HedgeEm AR Business Card
===================

A unity project for iOS and Android deployment.

To download the source code visit [haukebrinkmann.de/hedgeem_ar/unityproject.zip](http://haukebrinkmann.de/hedgeem_ar/unityproject.zip)

#### Running the app

###### Unity Editor
1. A camera is required
2. Hit Play Button

###### Creating an iOS Build
1. You need a mac or macbook and Unity 5.6x
2. In Unity press File -> Build Settings
3. Make sure iOS is choosen as platform
4. Hit "Player Settings"
5. Under Other Settings -> Identification up the version number and build
6. In the Build Settings hit "Build"
7. Choose a Folder to save the project

###### Upload the app to the iOS store
1. You need a mac or macbook and xCode
2. Open the just built Version by doubleclicking "Unity-iPhone.xcodeproj" in your project Folder
3. In the Top Left corner click the Element "Unity-iPhone" (The root of your file tree)
4. In General -> Signing check Automatically manage Signing
5. Choose Qeetoto Ltd as Team (If you don't have this option, add your apple account to the team and restart xCode)
6. In the Top Menu Bar choose "Generic iOS Device" next to the stop Identification
7. In the Menu klick Product -> Archive
8. Wait for the build to process and press "Upload to App Store"
9. A new context menu will pop up. Go through this and proceed to upload the Archive
10. Go to the app in [itunesconnect](https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1254970630/activity/ios/builds) - you should see your build within 10 minues. It will take a while before you can use it.
