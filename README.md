# dotnetcore-docker-haproxy

Sample app showing an aspnetcore website connecting to 5 instances of an aspnetcore api load-balanced round robin style via haproxy

![Compose](http://i.imgur.com/Owk9CHw.png)

## Getting started

Run `.\run.ps1` with the `-Build` switch

    > .\run.ps1 -Build

This will trigger a publish on the two aspnetcore apps and then build all the docker images

Then run `.\run.ps1` with the `-Compose` switch

    > .\run.ps1 -Compose

Launch a browser at http://docker:5000 and each refresh should return a different machine name

## Cleaning up

Run `.\run.ps1` with the `-Clean` switch

    > .\run.ps1 -Clean
