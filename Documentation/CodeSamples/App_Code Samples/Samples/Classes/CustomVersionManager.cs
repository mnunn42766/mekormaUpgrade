using System;
using System.Linq;

using CMS;
using CMS.DocumentEngine;
using CMS.EventLog;

// Custom manager registration. Uncomment the following line to enable the custom manager.
//[assembly: RegisterCustomManager(typeof(CustomVersionManager))]

/// <summary>
/// Sample custom cache helper, does log an event upon cache item removal.
/// </summary>
public class CustomVersionManager : VersionManager
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CustomVersionManager()
        : base(null)
    {
    }


    /// <summary>
    /// Undo all operations made during last checkout.
    /// </summary>
    /// <param name="node">Document node</param>
    protected override void UndoCheckOutInternal(TreeNode node)
    {
        base.UndoCheckOutInternal(node);

        // Log the event that the user was updated
        EventLogProvider.LogEvent(EventType.INFORMATION, "CustomVersionManager", "UndoCheckOut", "The page '" + node.DocumentName + "' checkout was undone.", null);
    }
}
