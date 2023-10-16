
<h1 align="center">
  <img src="https://user-images.githubusercontent.com/2679513/131189167-18ea5fe1-c578-47f6-9785-3748178e4312.png" width="150px"/><br/>
  Speckle connector for TopSolid
</h1>
<h3 align="center">
   Speckle is an open source data platform for interoperability in AEC.
</h3>
<br>
<p align="center">
<a href="https://speckle.community"><img src="https://img.shields.io/discourse/users?server=https%3A%2F%2Fspeckle.community&amp;style=flat-square&amp;logo=discourse&amp;logoColor=white" alt="Community forum users"></a> 
<a href="https://speckle.systems"><img src="https://img.shields.io/badge/https://-speckle.systems-royalblue?style=flat-square" alt="website"></a> <a href="https://speckle.guide/dev/"><img src="https://img.shields.io/badge/docs-speckle.guide-orange?style=flat-square&amp;logo=read-the-docs&amp;logoColor=white" alt="docs"></a>

</p>


<br><br>

# About the connector for TopSolid

What is TopSolid Connector ? :popcorn:  
[Check our videos on YouTube](https://www.youtube.com/@hackingbim4954)



![Untitled](https://user-images.githubusercontent.com/2679513/132021739-15140299-624d-4410-98dc-b6ae6d9027ab.png)

<br>

TopSolid is a CAD-CAM software that allows a continuous workflow from Design to Manufacturing [TopSolid](https://www.topsolid.fr/)
should be installed with a valid license.


The source code is available in the following repo :
[ENAC-CNPA/speckle-sharp](https://github.com/ENAC-CNPA/speckle-sharp)


Note : Work is still under progress


<br>

---

<br>
<div align="center">
    <img src="topsolid.png" width="150px"/>
</div>

## User Setup

1. Download the setup file : 
  - 7.16 : [Binaries/Speckle_ConnectorTopSolid_v7.16.zip](https://github.com/ENAC-CNPA/Speckle.TopSolid/raw/main/Binaries/Speckle_ConnectorTopSolid_v7.16.zip)
  - 7.17 : [Binaries/Speckle_ConnectorTopSolid_v7.17.zip](https://github.com/ENAC-CNPA/Speckle.TopSolid/raw/main/Binaries/Speckle_ConnectorTopSolid_v7.17.zip)
  
2. Open the zip file and launch the installation (administrator rights are required)


<br>

---

<br>

## Development Setup


1. Clone the repo in "C:\Sources\" : 

     `git clone https://github.com/ENAC-CNPA/speckle-sharp.git`

     Result output folder :
```
   C:\Sources\speckle-sharp
```

2. Open the solution in Visual Studio 2022+ (for 7.1(X) : your TopSolid version) : `C:\Sources\speckle-sharp\ConnectorTopSolid\ConnectorTopSolid71X\ConnectorTopSolid.sln`

3. Check the TopSolid Dev folder is in : `C:\Sources\Topsolid 7.1X\Debug x64` **

    ** The debug files are provided by the TopSolid ads team : [ads.topsolid.com](https://ads.topsolid.com)



<br>

---

<br>


## License
Unless otherwise described, the code in this repository is licensed under the Apache-2.0 License. Please note that some modules, extensions or code herein might be otherwise licensed. This is indicated either in the root of the containing folder under a different license file, or in the respective file's header. If you have any questions, don't hesitate to get in touch with us via email.


## Contact us
We are available for all questions and suggestions !

[CNPA Labs](https://www.epfl.ch/labs/cnpa/)