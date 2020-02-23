using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class ShareSheetScript : MonoBehaviour
{
    public eShareOptions[] m_excludedOptions; 

    public void shareURLUsingShareSheet()
    {
        ShareSheet _shareSheet = new ShareSheet();
        _shareSheet.Text = LocalizationManager.instance.GetLocalizedText("share_text"); 
        _shareSheet.URL = "https://www.friendlypixel.com";

        _shareSheet.ExcludedShareOptions = m_excludedOptions; 

        NPBinding.UI.SetPopoverPointAtLastTouchPosition(); 
        NPBinding.Sharing.ShowView(_shareSheet, finishedSharing);
    }

    private void finishedSharing(eShareResult result)
    {
        print("sucessfully shared link");
    }
}
