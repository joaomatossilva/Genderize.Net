Genderize.Net
===================
[https://genderize.io/](https://genderize.io/)

[![NuGet Version](http://img.shields.io/nuget/v/Genderize.Net.svg?style=flat)](https://www.nuget.org/packages/Genderize.Net/) 
[![MyGet Pre Release](https://img.shields.io/myget/genderizedotnet/vpre/Genderize.Net.svg)](https://www.myget.org/feed/genderizedotnet/package/nuget/Genderize.Net)
[![AppVeyor](https://img.shields.io/appveyor/ci/kappy/genderize-net.svg)](https://ci.appveyor.com/project/kappy/genderize-net)
[![GitHub contributors](https://img.shields.io/github/contributors/joaomatossilva/Genderize.Net.svg)](https://github.com/joaomatossilva/Genderize.Net)


A client wrapper of the genderize.io API - Genderize.io determines the gender of a first name.

## Stuff still needed for version 1.0

+ Handle of the error cases


## Usage

A simple way to use the client is simply like this

    var client = new GenderizeClient();
    var result = await client.GetNameGender("João");
    Assert.AreEqual(Gender.Male, result.Gender);