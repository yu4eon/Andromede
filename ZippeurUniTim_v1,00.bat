@echo off


rem // INSTRUCTIONS /////////////////////////////////////////////////////////////////

rem // Le ZippeurUniTim est un script con�u pour zipper facilement un projet Unity.

rem // Pr�requis: le logiciel 7-zip doit �tre install�.

rem // Utilisation: le fichier .bat doit �tre plac� dans le dossier du projet Unity
rem // (� c�t� des dossiers assets, etc.), puis il faut l'ex�cuter (double-clic).


rem // � PROPOS /////////////////////////////////////////////////////////////////////

rem // Ce script a �t� d�velopp� par Jonathan Tremblay (jtrembla@cstj.qc.ca)
rem // Il est offert sous licence CC0: il peut �tre modifi� et redistribu� :-)
rem // https://creativecommons.org/share-your-work/public-domain/cc0/


rem // INFOS DE VERSION /////////////////////////////////////////////////////////////

set "Version=V1.00"
set "Date=2022-08"
rem // Cette version permet de conserver la derni�re sc�ne ouverte dans Unity
rem // (m�me si le reste du dossier Library n'est pas inclus!)


rem // CONFIGURATION AVANC�E ////////////////////////////////////////////////////////

rem ExclureLibrary:    1 permet d'exclure le dossier Library (0 pour l'inclure)*
set ExclureLibrary=1

rem ExclureGit:        1 permet d'exclure le dossier .git (0 pour l'inclure)*
set ExclureGit=1

rem ExclureZip:        1 permet d'exclure les fichiers .zip (0 pour les inclure)*
set ExclureZip=1

rem *Les exclusions cr�ent des archives plus l�g�res et rapides (surtout Library)!

rem NiveauCompression: permet de choisir le niveau de compression (1, 3, 5, 7, 9)
rem                    1 (Fastest), 3 (Fast), 5 (Normal), 7 (Maximum), 9 (Ultra)
set NiveauCompression=7


rem // D�BUT DU PROGRAMME ///////////////////////////////////////////////////////////

rem Sauvegarde de l'encodage original:
for /f "tokens=2 delims=:." %%x in ('chcp') do set cp=%%x
rem Changement de l'encodage:
chcp 1252>nul

rem Variables pour la mise en forme du texte: 
set "Gras=[1m"
set "Norm=[0m"
set "Rouge=[91m"
set "Vert=[92m"
set "FondRouge=[41;1m"
set "FondVert=[42;1m"

:PAGETITRE
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@         @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo @@@@@@@@@@@@@@@@@@@@@                     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo @@@@@@  @@@@@@@@@@      @@@@@@@@@@  @@@@     @@@@@@@@@@  @@@@@@@             @@@
echo @@@@@  @@@  @@@@    @@@@@@@@@@@@@@@@@@@@@@@    @@@   @@@  @@@@@@             @@@
echo @@@@  @@@   @@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@   @@   @@@@@   %Gras%ZIPPEUR%Norm%   @@@
echo @@@   @@   @@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@  @@@  @@@@@   %Gras%UNITIM %Norm%   @@@
echo @@@  @@@  @@@   @@@@@@@@@@@@@    @@@              @@   @@   @@@@             @@@
echo @@@  @@@  @@@   @@@@@   @@@@@@@ @  @@@@@@@@@@@@   @@@  @@@  @@@@   TIM       @@@
echo @@@  @@@  @@@   @@@@@ @@@@@@@@@                   @@   @@@  @@@@   CSTJ      @@@
echo @@@  @@@   @@   @@@@@              @  @          @@@   @@   @@@@   %Version%     @@@
echo @@@@  @@@  @@@@@@ @@@@            @     @       @@@   @@@  @@@@@   %Date%   @@@
echo @@@@   @@@  @@@@   @@@@                 @      @@@   @@@  @@@@@@   CC0       @@@
echo @@@@@   @@@@@@@@@     @@@@            @@     @@@@@@@@@@  @@@@@@@             @@@
echo @@@@@@@@@@@@@@@@@@@@      @@@@@@@@@@@      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo @@@@@@@@@@@@@@@@@@@@@@@@               @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.

rem Variable pour indiquer les exclusions:
set "DossiersExclus=Logs, Temp."

rem D�tection des exclusions et ajustement des espacements:
if %ExclureLibrary% neq 0 (
	set "DossiersExclus=Library, %DossiersExclus%"
)
if %ExclureGit% neq 0 (
	set "DossiersExclus=.git, %DossiersExclus%"
)
if %ExclureZip% neq 0 (
	set "DossiersExclus=*.zip, %DossiersExclus%"
)

rem Variable pour afficher la ligne des exclusions:
set "DossiersExclus=  (Exclusions : %DossiersExclus%)"

echo   Permet de cr�er un fichier zip optimis�, � partir d'un dossier projet Unity.
echo %DossiersExclus%

rem Ajustement du dossier actuel (utile en contexte admin):
setlocal enableextensions
cd /d "%~dp0"

rem Identification du nom du dossier:
for %%I in (.) do set NomD=%%~nxI

rem Variable du nom du fichier archive:
set Fichier=%NomD%.zip

rem Ajout du chemin de 7-Zip:
set path7z="C:\Program Files\7-Zip"
set PATH=%PATH%;%path7z%

rem Recherche le dossier de 7zip:
if NOT exist %path7z% goto ERREUR7ZINSTAL

echo.
echo   Le contenu du dossier suivant va �tre archiv� :
echo   %Gras%%cd%%Norm%

rem Recherche du sous-dossier Assets:
if NOT exist "%cd%\Assets" goto ERREURUNITY

rem Recherche d'une archive du m�me nom:
echo.
if exist "%cd%\%Fichier%" goto REMPLACER

:CREER
echo   Le fichier %FondVert%%Fichier%%Norm% sera %Vert%cr��%Norm%.
goto QUESTION

:REMPLACER
echo   Attention, le fichier %FondRouge%%Fichier%%Norm% sera %Rouge%remplac�%Norm%.

:QUESTION
echo.
set /p mode=  Souhaitez-vous continuer? (%Gras%[O]%Norm%, N) 

rem Si c'est NON, on passe � l'�tiquette ARRET
if /i "%mode%" == "N" goto ARRET
if /i "%mode%" == "Non" goto ARRET

rem Par d�faut, c'est le mode automatique...

:AUTOMATIQUE
rem Retour temporaire � l'encodage initial (pour les messages d'erreur de 7zip):
chcp %cp%>nul

rem Supression du fichier si existant:
if exist "%cd%\%Fichier%" (
	del "%cd%\%Fichier%"
	rem Test pour d�tecter une erreur de suppression:
	if exist "%cd%\%Fichier%" goto ERREURDEL
)

rem La commande de base 7-zip (ajoute les fichiers dans l'archive, sauf Logs et Temp):
set "Commande=7z a -tzip ^"%Fichier%^" ^"%cd%^" -mx%NiveauCompression% -xr!Logs -xr!Temp"

rem R�duction du verbillage de 7-zip lors de la compression:
set "Commande=%Commande% -bb0 -bso0"

rem Ajout des exclusions, selon les r�glages:
if %ExclureLibrary% neq 0 set "Commande=%Commande% -xr!Library"
if %ExclureGit% neq 0 set "Commande=%Commande% -xr!.git"
if %ExclureZip% neq 0 set "Commande=%Commande% -xr!*.zip"
rem Ex�cution de la commande 7-zip:
%Commande%
rem M�morisation du code d'erreur:
set MemErreur=%ERRORLEVEL%

rem Si une erreur survient:
if %MemErreur% neq 0 goto ERREUR7ZA

rem Ajout du fichier indiquant la derni�re sc�ne ouverte:
set "FichierScene=Library\LastSceneManagerSetup.txt"
if %ExclureLibrary% neq 0 (
	if exist "%cd%\%FichierScene%" (
		goto AJOUTER
	)
)
goto VERIFIER

:AJOUTER
cd ..
7z a -tzip ^"%NomD%\%Fichier%^" ^"%NomD%\%FichierScene%^" -bb0 -bso0
cd %NomD%

:VERIFIER
rem M�morisation du code d'erreur:
set MemErreur=%ERRORLEVEL%

rem Si une erreur survient:
if %MemErreur% neq 0 goto ERREUR7ZA

rem Retour � l'encodage pour nos messages en fran�ais:
chcp 1252>nul

echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.
echo   %Vert%SUCC�S ^! %Norm%
echo   Fichier r�sultant: %Gras%%Fichier%%Norm%
echo   Merci d'avoir utilis� le ZippeurUniTim ^!
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
goto FIN

:ARRET
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.
echo   %Gras%INTERRUPTION ^! %Norm%
echo   Arr�t du script. Aucun fichier modifi�.
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
goto FIN

:ERREURUNITY
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.
echo   %Rouge%ERREUR ^! %Norm%
echo   Le dossier ne semble pas �tre un projet Unity (pas de sous-dossier Assets).
echo   Indice: %Gras%Le fichier .BAT doit �tre � l'int�rieur du dossier du projet Unity.%Norm% 
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
goto FIN

:ERREURDEL
rem Retour � l'encodage pour nos messages en fran�ais:
chcp 1252>nul
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.
echo   %Rouge%�CHEC ^! %Norm%
echo   Erreur lors de la suppression du fichier (utilis� par une autre application?)
echo   Suggestion: Supprimez manuellement le fichier %Gras%%Fichier%%Norm%
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
goto FIN

:ERREUR7ZINSTAL
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.
echo   %Rouge%ERREUR ^! %Norm%
echo   Pour utiliser ce script, %Gras%7-zip doit �tre install�%Norm% dans le dossier :
echo   %path7z%
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
goto FIN

:ERREUR7ZA
rem Retour � l'encodage pour nos messages en fran�ais:
chcp 1252>nul
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
echo.
echo   %Rouge%�CHEC ^! %Norm%
echo   Erreur 7-zip #%MemErreur% lors de la compression.
echo.
echo @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
goto FIN

:FIN
echo.
rem Retour � l'encodage initial:
chcp %cp%>nul
pause