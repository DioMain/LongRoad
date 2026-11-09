using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LongRoad.Managers
{
    public class AudioManager : LongRoadBehaviour
    {
        public enum CommonSfxClip
        {
            Click, Eat
        }

        public const float MIN_MIXER_VOLUME = -60f;
        public const float MAX_MIXER_VOLUME = 0;

        [Header("Mixers:")]
        [SerializeField]
        private AudioMixer sfxMixer;
        [SerializeField]
        private AudioMixer bgmMixer;
        [Header("Sources:")]
        [SerializeField]
        private AudioSource musicSource;
        [SerializeField]
        private List<AudioSource> sfxSlots;
        [Header("Common sfx clips:")]
        [SerializeField]
        private AudioClip clickSfx;
        [SerializeField]
        private AudioClip eatSfx;

        public float BgmVolume
        {
            get
            {
                bgmMixer.GetFloat("Volume", out float mixerVolume);

                return ToNormalizedVolume(mixerVolume);
            }

            set
            {
                bgmMixer.SetFloat("Volume", ToMixerVolume(value));
            }
        }

        public float SfxVolume
        {
            get
            {
                sfxMixer.GetFloat("Volume", out float mixerVolume);

                return ToNormalizedVolume(mixerVolume);
            }

            set
            {
                sfxMixer.SetFloat("Volume", ToMixerVolume(value));
            }
        }

        public void SetMusicClip(AudioClip clip)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(CommonSfxClip commonSfx)
        {
            switch (commonSfx)
            {
                case CommonSfxClip.Click: PlaySfx(clickSfx); break;
                case CommonSfxClip.Eat: PlaySfx(eatSfx); break;
                default: throw new ApplicationException("Unknown common sfx");
            }
        }

        public void PlaySfx(AudioClip clip)
        {
            var freeSlot = sfxSlots.FirstOrDefault(slot => !slot.isPlaying);

            if (freeSlot == null)
            {
                Debug.LogWarning("All sfx slots is playing");
                return;
            }

            freeSlot.clip = clip;
            freeSlot.Play();
        }

        ///TODO: Необходимо придумать формалу что-бы звук убывал по кривой, а не линейно

        private float ToNormalizedVolume(float mixerVolume)
        {
            return (mixerVolume - MIN_MIXER_VOLUME) / (MAX_MIXER_VOLUME - MIN_MIXER_VOLUME);
        }

        private float ToMixerVolume(float normVolume)
        {
            return MIN_MIXER_VOLUME + normVolume * (MAX_MIXER_VOLUME - MIN_MIXER_VOLUME);
        }
    }
}
