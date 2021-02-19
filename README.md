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

o   Hasła użytkowników powinny być odpowiednio zapisane na bazie przy pomocy algorytmów haszujących (opcjonalnie) 

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