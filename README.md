[![Open in Codespaces](https://classroom.github.com/assets/launch-codespace-2972f46106e565e64193e422d61a12cf1da4916b45550586e14ef0a7c637dd04.svg)](https://classroom.github.com/open-in-codespaces?assignment_repo_id=16557477)



## Softversko rješenje GlamOffice

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Anamarija Dominiković | adominiko22@foi.hr | 0016160419 | adominiko22
Nika Laštro | nlastro22@foi.hr | 0016158081  | nlastro22

## Opis domene
Domena razvoja softvera za praćenje rada kozmetičkog salona obuhvaća sve ključne aspekte organizacije i upravljanja jednim kozmetičkim salonom koji se u današnje vrijeme još uvijek mogu izvoditi na papiru. Ovom je softveru cilj olakšati praćenje izvođenja kozmetičkih tretmana, izradu rasporeda zaposlenika, upravljanje tretmanima, zaposlenicima i klijentima, omogućiti rezervaciju tretmana, izdavati račune, izvještaje i potvrde o rezervacijama termina klijentima. Administratorski aspekt uključuje praćenje i evidentiranje podataka o zaposlenicima, upis te praćenje i evidentiranje kozmetičkih tretmana i podataka o klijentima. Evidencija i praćenje izvođenja tretmana omogućuje generiranje izvještaja i statističkih podataka u određenom vremenskom rasponu. Sigurnim pristupom podacima i definiranjem pristupa različitim korisničkim ulogama, osigurava se povjerljivost i integritet informacija. Integracijom s online bazom podataka putem ADO.NET-a i .NET Framework-a omogućava se pouzdana pohrana i upravljanje podacima, čime se olakšava svakodnevno upravljanje i očuvanje podataka kozmetičkog salona. Sve navedene komponente zajedno čine važnu domenu za razvoj softverskog rješenja koje će uvelike unaprijediti kvalitetu pružanja usluga i zadovoljstvo svih dionika, uključujući zaposlenike, klijente i vlasnike kozmetičkih salona.

## Specifikacija projekta


Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Registracija i prijava zaposlenika | Sustav će omogućiti registraciju zaposlenika koji nemaju korisnički profil, čime će im se omogućiti prijava nakon dovršene registracije | Nika Laštro
F02 | Administracija i upravljanje zaposlenicima | Sustav će omogućiti kreiranje, uređivanje, brisanje i prikaz podataka o zaposlenicima kozmetičkog salona. | Anamarija Dominiković
F03 |
F04 |
F05 | Rezervacija termina tretmana | Sustav će omogućiti kreiranje i izmjenu rezervacija kozmetičkih tretmana koje klijenti zatraže. | Anamarija Dominiković
F06 |
F07 | Kreiranje i pregled rasporeda za zaposlenike (CRUD) | Sustav će omogućiti kreiranje rasporeda zaposlenika od strane administratora,  pregled termina i obaveza, ažuriranje rasporeda svih zaposlenika prema potrebama salona te brisanje termina. | Nika Laštro
F08 |
F09 |Statistički prikaz potražnje za kozmetičkim tretmanima | Sustav će omogućiti statistički pregled usluga koje su se najčešće izvodile, s ciljem isticanja najpopularnijih usluga. | Nika Laštro
F10 |
F11 | Izvještaj o uspješnosti tretmana | Sustav će omogućiti kreiranje izvještaja o uspješnosti odrađenih tretmana, uz mogućnost pisanja napomena ili detalja vrijednih pamćenja. | Anamarija Dominiković
F12 |
F13 |

## Tehnologije i oprema
Pri implementaciji softverskog rješenja za praćenje rada kozmetičkog salona koristit će se niz tehnologija, alata i opreme kako bi se osigurala kvaliteta, funkcionalnost i sigurnost rješenja. Ovdje je popis tih elemenata:

* [*Programski jezik C#*](https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/?view=vs-2022): Korišten za razvoj glavnog softverskog rješenja.

* [*Windows Forms .NET*](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/overview/?view=netdesktop-7.0): Korišten za razvoj grafičkog korisničkog sučelja, pružajući bogat, stabilan i interaktivan dizajn.

* [*Visual Studio*](https://visualstudio.microsoft.com/): Kao integrirano razvojno okruženje za programiranje u C# i Windows Presentation Foundation (WPF) .NET.

* [*SQLite, MySQL*](https://sqlitebrowser.org/): Za pohranu podataka o djeci, osoblju, rasporedu, aktivnostima i administrativnim informacijama.

* [*ADO.NET*](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview): Za integraciju i upravljanje online bazom podataka, omogućujući siguran pristup i manipulaciju podacima.

* [*Entity Framework*](https://learn.microsoft.com/en-us/ef/): Za olakšavanje upita i pristupa podacima u aplikaciji.

* [*SMTP server*](https://support.google.com/a/answer/176600?hl=en): Za slanje e-mail obavijesti roditeljima.

* [*Operativni sustav Windows*](https://www.microsoft.com/en-us/windows?r=1): Kako bi aplikacija bila kompatibilna s okruženjem većine korisnika.

* [*Projektni alati (Gantt Chart)*](https://www.onlinegantt.com/#/gantt): Za upravljanje projektom, planiranje aktivnosti, i praćenje napretka.

* [*Version Control Platform (GitHub)*](https://github.com/): Za upravljanje izvorima koda, verzioniranje i timsku suradnju.

* [*Visual Paradigm*](https://www.visual-paradigm.com/whats-new/): Za dizajniranje i kreiranje dijagrama

* [*Dokumentacijski alati (Microsoft Office Education)*](https://www.microsoft.com/en-us/education/products/microsoft-365): Za izradu tehničke dokumentacije, uputa za korištenje i dokumentacije korisnika.

Svi korišteni alati i tehnologije su javno dostupni i imaju odgovarajuće licence kako bi se osigurala legalnost i usklađenost s pravilima i propisima. Za tehnologije koje nisu standardno dostupne (npr. Entity Framework) u dokumentaciji će biti navedeni načini instalacije i korištenja.
