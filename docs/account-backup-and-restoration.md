# Psyche

### Target: AB.0XY.00Z

<table>
    <tr>
        <td valign="top">
            <h4><a href="../README.md">Revisions</a></h4>
            <h4>Site Map</h4>
            <ul>
                <li><a href="account-authentication.md">Account Authentication</a></li>
                <li><a href="account-backup-and-restoration.md">Account Backup and Restoration</a></li>
                <li><a href="area-exploration.md">Area Exploration</a></li>
                <li><a href="choice-based-narrative.md">Choice-based Narrative</a></li>
                <ul>
                    <li><a href="ruin-restoration.md">Ruin Restoration</a></li>
                </ul>
                <li><a href="progress-control.md">Progress Control</a></li>
                <ul>
                    <li><a href="progress-saving.md">Progress Saving</a></li>
                    <li><a href="progress-loading.md">Progress Loading</a></li>
                </ul>
            </ul>
            <br>
        </td>
        <td valign= "top">   
        <a href="https://github.com/Jhanez27/psyche">Home</a> &gt; <a href="https://github.com/Jhanez27/psyche/blob/main/docs/account-backup-and-restoration.md">Account Backup and Restoration</a>
        <br> <br>
            <img src="images/account backup (1).png">
          <h3>Account Backup and Restoration</h3>
            <span>Save files contain account details and progress including game progression and transactions, and players can back up their progress into these save files. Players can also restore their account progress by retrieving the latest save file. Players will be prompted to ensure the overwriting of their existing save files and accounts for backing up and restoring, respectively.</span>
            <h3>Use Case Scenario</h3>
             <table border="1">
        <tr>
            <th>Use Case</th>
            <th>Account Backup and Restoration</th>
        </tr>
        <tr>
            <th>Actor(s)</th>
            <td>Player</td>
        </tr>
        <tr>
            <th>Goal</th>
            <td>Successfully back up and restore player's current progress.</td>
        </tr>
        <tr>
            <th>Preconditions</th>
            <td>The player has an existing save files containing his progress data.</td>
        </tr>
        <tr>
            <th>Main Scenario</th>
            <td>
                  Backup <br>
                1. The player navigates to the setting menu.<br>
                2. The player selects to back up current game progress.<br>
                3. The system will prompt the player to confirm overwriting the existing back up file.<br>
                4. The player confirms it, and the system will then proceed with overwriting the file.<br>
                  Restoration<br>
                1. The player navigates to the setting menu.<br>
                2. The player selects to back up current game progress.<br>
                3. The system will prompt the player to confirm restoring this saved progress, potentially overwriting the current game progress.<br>
                4. The player confirms it, and the system restores the latest backup.<br>
            </td>
        </tr>
        <tr>
            <th>Outcome</th>
            <td><span>The player successfully backs up and restore his progress. </span></td>
        </tr>
    </table>
        </td>
    </tr>
    <tr>
        <td colspan="2"><p align="center">© 2025 Spheron</p>
</td>
    </tr>
</table>
