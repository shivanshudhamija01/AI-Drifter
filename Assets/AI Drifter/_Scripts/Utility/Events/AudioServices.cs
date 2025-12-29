using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioServices : GenericSingleton<AudioServices>
{
    public EventController<Enums.Audios> PlayAudio;

    public AudioServices()
    {
        PlayAudio = new EventController<Enums.Audios>();
    }
}
