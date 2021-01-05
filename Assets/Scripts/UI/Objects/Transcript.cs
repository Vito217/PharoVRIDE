﻿using LoggingModule;
using SaveAndLoad;

public class Transcript : InitializeBehaviour
{
    public override void onClose()
    {
        if(loadingWheel == null || !loadingWheel.activeSelf)
        {
            SaveAndLoadModule.transcripts.Remove(this);
            InteractionLogger.Discount("Transcript", GetInstanceID().ToString());
            Destroy(gameObject);
        }
    }

    public override void innerBehaviour()
    {
        field.text = SaveAndLoadModule.transcriptContents;
    }
}
