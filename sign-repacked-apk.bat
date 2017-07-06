rem 给重新打包的apk签名，记得删除原包里的Menifest里的两个cert证书文件
echo passphrase: android
jarsigner -keystore debug.keystore -signedjar jigsaw-repacked-signed.apk jigsaw-repacked.apk androiddebugkey