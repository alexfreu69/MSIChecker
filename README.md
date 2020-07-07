# MSIChecker
Checks the integrity of the Windows MSI infrastructur and allows to repair some issues.
*BE CAREFUL* - you can break future updates or uninstallations if you don't know what you're doing!

Usage:
Run MSIChecker and click at "Scan". You will see a list of installed applications for the SYSTEM account (S-1-5-18) and other users. Check for the "Status" column. If there is anything else than "OK" listed, like #MISSING# or #ORPHANED#, then something needs to be done to repair the MSI infrastructure for the application.

#MISSING#
The required MSI file is missing from the c:\windows\installer folder. You may want to download the original patch from https://www.catalog.update.microsoft.com/Home.aspx, extract it to c:\windows\installer and rename it to the name, listed in the "File Name" column.
You can right-click the entry and run "Search GUID" to see what application is hiding under the GUID.

#ORPHANED#
There is a MSI file without the required information in the registry. You can right-click the line and run "Remove File"

Context Menu:
* Search GUID - searches for the GUID in Google
* MSI/MSP properties - opens the Windows Explorer file properties dialog window
* Uninstall - runs MSIEXEC /X or MSIEXEC /uninstall
* Remove file - deletes the MSI file *ATTENTION* - don't do this for entries with the status OK - this will break future updates for the selected application!
* Cleanup Registry - Deletes traces of the application in the registry. The MSI file and the application itself will remain in place. *ATTENTION* - Don't do this if the status is OK
