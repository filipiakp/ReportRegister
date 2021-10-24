# Report Register

## Description

Simple ASP.NET Core MVC for managing reports. 

### Feaures
* Email notifications about reports updates
* 2 built-in employees accounts
* Replies to reports
* File attachments to report
* Containerization support
* EntityFramework Core
* Identity
* ASP.NET Core 3.1
* MailKit

## How to build?
Clone repository, e.g.:
git clone https://github.com/filipiakp/ReportRegister.git

There are 2 ways to build this project in dev environment.

First is to simply run it without containers. Remember to setup your database first.
This method requires to manually set environment variables or write them to appsettings.json(CUSTOMCONNSTR has to be standard connection string definition). Names are listed below in example docker-compose file. 

Second is to fill docker-compose file and save it to created ReportRegiser folder.
Then you can run it with command:
docker-compose up -d

Make sure both containers are running:
docker -ps

If reportregister_app is stopped(probably started too soon, before db was fully started), start it again with optional -a for showing output of container:
docker start -a reportregister_app

When application is up you can start using it by typing localhost in your browser.

Example docker-compose.yml file:
```YAML
version: '3.4'

services:
  reportregister_app:
    container_name: reportregister_app
    image: ${DOCKER_REGISTRY-}reportregister
    build:
      context: .
      dockerfile: Dockerfile
    ports:
    - "80:80"
    - "443:443"
    environment:
      employee1_email: example@mail.com
      employee1_password: strongPassword, otherwise account won't be created
      employee2_email: example@mail.com
      employee2_password: strongPassword
      SMTP_Mail: emailFrom
      SMTP_DisplayName: reportregister
      SMTP_Password: passwordToEmailFrom
      SMTP_Host: host
      SMTP_Port: port
      CUSTOMCONNSTR_DefaultConnection: "Data Source=reportregister_db;Initial Catalog=ReportRegisterDB;Persist Security Info=True;User ID=User;Password=Password;MultipleActiveResultSets=True;TrustServerCertificate=True"
    depends_on:
      - reportregister_db
  reportregister_db:
    container_name: reportregister_db
    image: mcr.microsoft.com/mssql/server:2019-latest
    ports:
    - "1433:1433"
    environment:
      SA_PASSWORD: "strongPassword"
      ACCEPT_EULA: "Y"
```

# Zadanie rekrutacyjne


## Opis 

Celem zadania jest napisanie systemu do obsługi zgłoszeń klientów. Zarejestrowany i zalogowany użytkownik może utworzyć nowe zgłoszenie, nadać zgłoszeniu tytuł oraz opisać sytuacje. Opcjonalnie może też dodać pliki w postaci pdf/jpg. 

Przetwarzaniem zgłoszeń zajmują się pracownicy zalogowani do odpowiednich kont. Pracownik może odpisać na zgłoszenie oraz zmienić jego status 

Klient powinien posiadać podgląd wszystkich swoich zgłoszeń i nie mieć dostępu do zgłoszeń innych użytkowników 


## Wymagania funkcjonalne 

·        Użytkownicy 

o   Logowanie do systemu 

o   Wylogowanie 

o   Role użytkowników (min. dwie role, pracownik oraz użytkownik) 

o   Minimum dwóch pracowników automatycznie dodawanych do bazy przy pierwszym uruchomieniu aplikacji 

o   Rejestracja klientów 

o   Potwierdzenie rejestracji email (opcjonalnie) 

o   Powiadomienia email (opcjonalnie) 

o   Hasła użytkowników powinny być odpowiednio zapisane w bazie przy pomocy algorytmów haszujących (opcjonalnie) 

·        Zgłoszenia 

o   Zgłoszenia zawsze tworzy klient 

o   Zgłoszenie posiada tytuł, datę utworzenia oraz status (nowy, w trakcie rozpatrywania, zamknięty) 

o   Opcjonalnie – zgłoszenie może zawierać pliki w formie pdf/jpg 

o   Opcjonalnie – dowolna zmiana zgłoszenia przez pracownika wysyła maila do klienta 

o   Zmiany w zgłoszeniu może nanieść dowolny pracownik 

·        Pracownicy 

o   Pracownik powinien mieć możliwość podglądu oraz modyfikacji dowolnego zgłoszenia 


## Wymagania niefunkcjonalne 

·        Baza danych MSSQL 

·        Połączenie z bazą za pomocą EntityFramework 

·        Backend ASP .NET Core 3.1 (dowolny frontend) 

·        Repozytorium Git 

·        Użycie dependency injection  