# PlayFab Demo
*This sample was created on Unity 2019.4.24f1*

## Description
This sample demonstrates a simple single-player  game that utilizes PlayFab for:
* User Accounts
* User Title Data
* User Inventory
* Virtual currency
* Catalogs & items
* Events that trigger Cloudscript
* Player Statistics

## Using the Sample

### Requirements
The project is configured to expect specific title configuration. The title is pre-configured to use a custom PlayFab Title ID, and as such should work out-of-the-box. If you wish to adapt it to your own needs, please see the Configuration Section below.

### Basic Usage
After proceeding past the welcome page, you will be prompted to sign in to PlayFab. You can use an existing account, create a new one or sign in as a guest. You may also choose to be remembered to avoid this dialog in the future.

Playing the game consists of flying around an endless side-scrolling map, avoidinng planes and buildings and collecting as many coins as possible.

Coins are important, as they are used as a **Soft Currency** to buy lives. 
Lives will be regenerating every 2 minuts, only if the player has less than 5.

If all lives are lost, it is allways possible to buy a bundle of x3, for a certain amount of **coins**.

After every Game Over screen, the login menu will be prompt to enforce the player creating a **recoverable account**.

## Configuration
This sample relies the following being configured in the PlayFab developer portal.
Before you begin, locate the Catalog.json and CloudScript.js files in the PlayFab folder

1. Log in to [Game Manager](https://developer.playfab.com/).
2. Select your title.
3. In left navigation pane, select **Economy**, then select the **Currency** tab.
4. Select **New Currency**. Enter the following values to create a Virtual Currency:

  | Property | Value | Detail |
  | ---: | :---: | --- |
  | Code | LV | Abbreviation for our VC |
  | Name | Lives | Name of our VC |
  | Initial Deposit | 1 | ensure that the player can spin on their first login |
  | Recharge Rate | 720 |  Every 5 minutes: 24hr*60min / 2min |
  | Recharge Max | 5 | this caps the regeneration to the specified number |

5. Select **Save Currency**.
6. Add a second Virtual Currency to match the following parameters:

  | Property | Value | Detail |
  | ---: | :---: | --- |
  | Code | CO | Abbreviation for our VC |
  | Name | Coins | Name of our VC |
  | Initial Deposit | 15 | How many each player gets with a new account |

7. Select **Save Currency**.
8. Select the **Catalog** tab, the select **Upload JSON**.
9. Select the Catalog.json file, then select **Upload File**.
10. In left navigation pane, select **Automation**, then select **Revisions**.
11. Select **Upload New Revision**, select the CloudScript.js file and then select **Save as revision**.

### Leaderboards
The following Leaderboards are configured as Manual reset Sum statistics:

* games_played
* total_coins_session
* total_session_time

*The statistics are send to the server, but unfortunately I had no time to implement the leaderboars in the client*

### Automation Rules
The following rules need to be created to link the client calls to the cloud script:

* update_statstic: Custom Event, Execute Cloud Script, updateStatistic
* update_statstics: Custom Event, Execute Cloud Script, updateStatistics
