using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ININ.Alliances.DotNetIvr.Actions;

namespace ININ.Alliances.DotNetIvr
{
    internal class MenuManager
    {
        private static MenuManager _instance;

        private static MenuManager Instance
        {
            get { return _instance ?? (_instance = new MenuManager()); }
        }

        private readonly ActionResponse _menu;

        private MenuManager()
        {
            // Add main menu
            _menu = new PlayActionResponse
            {
                id = "MainGreeting",
                message = "Thank you for calling the dot net I V R. Press 1 to continue."
            };

            // Add get digit after main menu
            _menu.Children.Add(new GetDigitsActionResponse
            {
                id = "MainGreetingGetDigits"
            });

            // Add activity for "press 1"
            _menu.Children[0].Children.Add(new PlayActionResponse
            {
                id = "ThankYou",
                Digit = "1",
                message = "Great! Thank you."
            });
            _menu.Children[0].Children[0].Children.Add(new TransferToQueueActionResponse());
        }

        internal static ActionResponse GetNextAction(string lastactionid, string lastdigitsreceived)
        {
            try
            {
                // Start menu
                if (string.IsNullOrEmpty(lastactionid))
                    return Instance._menu;

                // Last item was error, disconnect
                if (lastactionid == "error")
                    return new DisconnectActionResponse();

                // Find previous menu item by id using a linq query
                var menuItem = FindActionById(Instance._menu, lastactionid);

                // Validate result from linq query
                if (menuItem == null)
                    throw new Exception("Unable to find action with ID " + lastactionid);

                // If only one child and a digit was not provided, return it now
                if (menuItem.Children.Count == 1 && menuItem.Children[0].Digit.Equals(lastdigitsreceived, StringComparison.InvariantCultureIgnoreCase))
                    return menuItem.Children[0];

                // Validate last digits received
                if (string.IsNullOrEmpty(lastdigitsreceived))
                    throw new Exception("Cannot find next menu option without digits received!");

                // Find selected child by Digit using a linq query
                var nextAction = menuItem.Children.FirstOrDefault(actionResponse =>
                    actionResponse.Digit.Equals(lastdigitsreceived, StringComparison.InvariantCultureIgnoreCase));

                // Validate result from linq query. No result means an invalid digit was pressed.
                if (nextAction == null)
                    //throw new Exception("Cannot find next menu option with parent " + lastactionid + " and digits " + lastdigitsreceived);
                    return new DisconnectActionResponse();

                // Return selected action
                return nextAction;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetNextAction: " + ex.Message);
                return new PlayActionResponse
                {
                    id = "error",
                    message = "An error occurred processing your request."
                };
            }
        }

        private static ActionResponse FindActionById(ActionResponse haystack, string id)
        {
            // Is the haystack the action?
            if (haystack.id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                return haystack;

            foreach (var child in haystack.Children)
            {
                // Is this child the action?
                if (child.id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                    return child;

                // Recursively look for action
                var childResult = FindActionById(child, id);
                if (childResult != null) return childResult;
            }

            // If we got here, we didn't find anything. Return null.
            return null;
        }
    }
}
