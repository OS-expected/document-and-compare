<h2>Document and Compare</h2>

<p align="center"><img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/github_1.png" width="550" alt="docAndCom logo"></p>

<table>
  <tbody>
    <tr>
      <td>
        timeframe
      </td>
      <td>17.04.2020 - 28.04.2020 (v1.0) <br/>
          29.04.2020 - 05.05.2020 (tests, fixes, documentation) <br/>
          11.07.2020 - 12.07.2020 (v1.1.1)
      </td>
    </tr>
    <tr>
      <td>
        supported languages
      </td>
      <td>
       <img src="https://img.shields.io/badge/-EN,%20PL-red?color=E41570&style=flat-square" alt="to do: add alt text"/>
      </td>
    </tr>
  </tbody>
</table>

<p align="justify">docAndCom developed as "Document and Compare" is app made in Xamarin.Forms that targets Android devices. Founder's assumption was to create solution that will make process of verifying changes between documented images much more simpler. User creates tags which then can be used to document images. After gathering satisfying amount of data under specific tag, user can generate PDF file in list or tabular form. App makes collecting data much more easier and there is no need in wasting time to match everything using external software(e.g. paint) like  for e.g. some people do to show effectiveness of using specific product. This is just an example how this app can be used. Any subject of interest can be: tagged, documented and then generated. Human imagination is the only restrictive limit here. On the bottom of the documentation you will find download section with important notes including <strong>safety</strong>. </p>

<h2>Screens</h2>
<h6>*Made using Android Emulator </h6>

| | | | |
| :---: | :---: | :---: | :---: |
| <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen1.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen2.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen3.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen4.png" alt="#toadd" height="300"/> |
| <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen5.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen6.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen7.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen8.png" alt="#toadd" height="300"/> |
| <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen9.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen10.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen11.png" alt="#toadd" height="300"/> | <img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/screen12.png" alt="#toadd" height="300"/> |
<!-- For image table, it's highly recommended to have the same resolution images. 
 To find best results(no stretches, equal cells), both axis should be adjusted manually. -->

<h2>Tested on</h2>

- Physical device: Huawei Y7, OS. Android 7.0, res. 720 x 1280
- Physical device: Huawei P20, OS. Android 9.0, res. 2244 x 1080
- Android emulator: H-DPI 4" phone, OS. Android 9.0, res. 480 x 800

<h2>Permissions needed</h2>

:white_check_mark: <strong>READ</strong> to access camera, gallery(documenting images), generated files(opening, checking if they exist) <br><br>
:white_check_mark: <strong>WRITE</strong> to save images documented through camera, generated pdf files, preferences and references stored in the database<br><br>
:white_check_mark: <strong>CAMERA</strong> to let user set new document image by taking a photo with camera <br>
<!-- If you did not specify icon, simply overwrite Id put between : : characters with desired icon name -->
<!-- Supported by GitHub icon list can be found here: https://gist.github.com/rxaviers/7360908 -->

<h2>Used libraries</h2>

- <a href="https://github.com/xamarin/Xamarin.Forms">Xamarin.Forms</a>
- <a href="https://github.com/sqlite/sqlite">SQLite</a>
- <a href="https://github.com/schourode/iTextSharp-LGPL">iTextSharp</a>
- <a href="https://iconify.design">Iconify icons</a>
- <a href="https://fontawesome.com/">Font Awesome icons</a>
- <a href="https://github.com/jamesmontemagno/MediaPlugin">Xam.Plugin.Media</a>
- <a href="https://github.com/lilcodelab/Xamarin.Plugin.Calendar">Xamarin.Plugin.Calendar</a>
- <a href="https://github.com/luberda-molinet/FFImageLoading">FFImageLoading</a>

<h2>Download & info</h2>

Minimum required Android version to use docAndCom: <strong>5.0 (Lollipop)</strong>

Targetted Android OS: <strong>9.0 (Pie)</strong>

<p align="justify">You can download the newest *.apk installation file by clicking <a href="https://github.com/trolit/document-and-compare/releases/download/1.1.1/docAndCom_1.1.1.apk">here</a>. <strong>Note that the original docAndCom solution developed by <a href="https://github.com/trolit">trolit</a></strong> is not currently available on any other pages delivering software than this GitHub repository. Guaranteed, safe way to obtain the app without suspicious modifications is to download it through <strong>the link given above</strong>! Other pages are unverified. Any information that user will add/generate through docAndCom is kept on local device storage and is not shared further without user's knowledge. App <strong>will never</strong> prompt for permissions other than detailed in the documentation(read, write, camera), show advertisements. docAndCom is <strong>NOT connecting</strong> to the Internet. If you wonder why there is no IOS version when Xamarin.Forms allows to build on both platforms with minimum changes, I don't own any IOS device to test the application. </p>

<p align="justify">To proceed with the downloaded apk, refer to the Android documentation <a href="https://developer.android.com/studio/publish#publishing-unknown">HERE</a> on how to enable 3rd party app installation(if you did not enable that option before) and then simply launch apk file while using Android device. </p>

<h2>License</h2>

<p align="justify">iTextSharp is licensed under LGPL, the rest of the project under standard MIT. (c) 2020 Paweł Idzikowski. Publishing the 1 to 1 solution on other sites must be discussed with it's founder first!</p>

<h2>Changelog</h2>

<h4>12.07.2020</h4>

- fixed wrong result message when image was deleted through DocumentPage(calendar)
- fixed how listview in tag section is rendered(more space for text)
- added to the tag section button that allows to see all images attached to that tag and manage them(demonstration below)

<img src="https://raw.githubusercontent.com/trolit/document-and-compare/storage/images/demo_1.1.1.gif" alt="#toadd" height="500"/>

<h4>12.05.2020</h4>

- edit help documentation(helpDocPt3 and helpDocPt5)


<br/>
<br/>

Template generated using <a href="https://github.com/trolit/EzGitDoc">EzGitDoc</a>
