using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public SuperAdventure()
        {
            InitializeComponent();

            Location location = new Engine.Location(1, "Home", "This is your house, fool.");

            _player = new Player(10, 10, 20, 0, 1);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void SuperAdventure_Load(object sender, EventArgs e)
        {

        }

        private void btnNorth_Click(object sender, EventArgs e)
        {

        }

        private void btnWest_Click(object sender, EventArgs e)
        {

        }

        private void btnSouth_Click(object sender, EventArgs e)
        {

        }

        private void btnEast_Click(object sender, EventArgs e)
        {

        }

        private void MoveTo(Location newLocation)
        {
            if(newLocation.ItemRequiredToEnter != null)
            {
                bool playerHasRequiredItem = false;

                foreach(InventoryItem ii in _player.Inventory)
                {
                    if(ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
                    {
                        // We found the required item
                        playerHasRequiredItem = true;
                        break; //Exit out of the foreach loop
                    }
                }

                if(!playerHasRequiredItem)
                {
                    //We didn't find the required item in their inventory, so display a message and stop trying to move
                    rtbMessages.Text += $"You must have a {newLocation.ItemRequiredToEnter.Name} to enter this location.{Environment.NewLine}";
                    return;
                }
            }

            // Update the player's current position
            _player.CurrentLocation = newLocation;

            // Show/Hide availble movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            // Display the current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            // Completely heal the player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Update Hit Points in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            // Does the location have a quest?
            if(newLocation.QuestAvailableHere != null)
            {
                // See if the player has already has the quest and if they've completed it
                bool playerAlreadyHasQuest = false;
                bool playerAlreadyCompletedQuest = false;

                foreach(PlayerQuest playerQuest in _player.Quests)
                {
                    if(playerQuest.Details.ID == newLocation.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;

                        if(playerQuest.IsCompleted)
                        {
                            playerAlreadyCompletedQuest = true;
                        }
                    }
                }

                // See if the player already has the quest
                if(playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if(!playerAlreadyCompletedQuest)
                    {
                        // See if the player has all of the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest = true;
                        
                        foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                        {
                            bool foundItemInPlayersInventory = false;

                            // Check each item in the player's inventory to see if they have it and enough of it
                            foreach(InventoryItem ii in _player.Inventory)
                            {
                                // The player has this item in their Inventory
                                if(ii.Details.ID == qci.Details.ID)
                                {
                                    foundItemInPlayersInventory = true;

                                    if(ii.Quantity < qci.Quantity)
                                    {
                                        // The player does not have enough of this item to complete the quest
                                        playerHasAllItemsToCompleteQuest = false;

                                        //no reason to continue checking
                                        break;
                                    }
                                    // we found the item, so don't check the rest of the player's Inventory
                                    break;
                                }
                            }

                            // If we didn't find the required item, set our variable and stop looking for other items
                            if(!foundItemInPlayersInventory)
                            {
                                playerHasAllItemsToCompleteQuest = false;
                                break;
                            }
                        }

                        if(playerHasAllItemsToCompleteQuest)
                        {
                            // Display Message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += $"You complete the {newLocation.QuestAvailableHere.Name} quest.{Environment.NewLine}";

                            // Remove quest items from inventory
                            foreach(QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                            {
                                foreach(InventoryItem ii in _player.Inventory)
                                {
                                    if(ii.Details.ID == qci.Details.ID)
                                    {
                                        // Subtract the quantity from the player's inventory that was needed to complete the quest
                                        ii.Quantity -= qci.Quantity;
                                        break;
                                    }
                                }
                            }

                            // Give Quest Rewards
                            bool addedItemToPlayerInventory = false;

                            foreach(InventoryItem ii in _player.Inventory)
                            {
                                if(ii.Details.ID == newLocation.QuestAvailableHere.RewardItem.ID)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }
    }
}
