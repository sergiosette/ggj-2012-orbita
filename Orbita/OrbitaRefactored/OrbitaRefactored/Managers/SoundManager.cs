using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace OrbitaRefactored
{
    public class SoundManager
    {

        /**
         * MUSIC
         */
        public const int MUSIC_GAMEPLAY     = 0;
        
        public int mCurrentMusic            = -1;

        private Song mMusicGameplay;


        
        private ContentManager mContent;
        

        private static SoundManager instance;

        
        /**************
         * FX
         * ************/
        //public const int FX_1 = 0;
        
        //private const int FX_MAX_COUNT = 1;

        private SoundEffect[] mEffects;// = new SoundEffect[MAX_FX_COUNT];


        private SoundManager(ContentManager contentManager)
        {
            mContent = contentManager;
            MediaPlayer.Volume = 0.5f;

            //loadSounds();
        }

        public static SoundManager getInstance(ContentManager contentManager)
        {

            if (instance == null)
                instance = new SoundManager(contentManager);
            return instance;

        }

        public void loadMusic()
        {

            //mMusicHeroiSertao = mContent.Load<Song>("musictest");//vilarejo");//Stage1.getInstance().getCurrentScene().getContent().Load<Song>("sounds\\music1");
            //Game1.print("LOADING MUSIC ROOM SOUNDS");

            mMusicGameplay =  mContent.Load<Song>("Musicas\\OrbitaLOOP");
            
            /*
            mEffects = new SoundEffect[2];
            mEffects[0] = mContent.Load<SoundEffect>("narracao");
            mEffects[1] = mContent.Load<SoundEffect>("iniciar");
            */
        }

        public void loadMainMenuSounds()
        {
           // mEffects = new SoundEffect[FX_MAX_COUNT];
           // mEffects[FX_INICIAR] = mContent.Load<SoundEffect>("iniciar");
        }

        public void playMusic(String resourceName)
        {
            //mMusicHeroiSertao = mContent.Load<Song>(resourceName);//vilarejo");//Stage1.getInstance().getCurrentScene().getContent().Load<Song>("sounds\\music1");
            //MediaPlayer.Play(mMusicHeroiSertao);
            //MediaPlayer.IsRepeating = true;
        }

        public void releaseSounds()
        {
            //mEffects[0].Dispose();
            //mEffects[1].Dispose();
        }

        private void diminuiVolume()
        {
            for (float x = 1; x > 0; x -= 0.01f)
                MediaPlayer.Volume -= x;
        }

        private void aumentaVolume()
        {
            for (float x = 0; x < 1; x += 0.0001f)
                MediaPlayer.Volume += x;
        }


        public void playFX(int soundId)
        {

           // if (mEffects[soundId].IsDisposed)
             //   mEffects[FX_STAR] = mContent.Load<SoundEffect>("BoulderHit");

            if (mEffects[soundId].IsDisposed)
            {
                mEffects[soundId].CreateInstance();
            }
            mEffects[soundId].Play();
            
        }

        public void stopFX(int soundId)
        {
            if(mEffects[soundId].IsDisposed == false)
            mEffects[soundId].Dispose();
        }

        public void playWAV(String name)
        {
            SoundEffect effect = mContent.Load<SoundEffect>(name);
            effect.Play();
        }


        public void playMusic(int musicId)
        {

            switch (musicId)
            {

                case MUSIC_GAMEPLAY:

                    MediaPlayer.Play(mMusicGameplay);
                    MediaPlayer.IsRepeating = true;
                    mCurrentMusic = MUSIC_GAMEPLAY;
                    break;

            }

        }

        public void stop()
        {

            MediaPlayer.Stop();
            mCurrentMusic = -1;
            //    break;
            /*
          case <<<<fx index>>>>:
              mEffects[<<FX_INDEX>>].Play();
              break;
         */
            //}

        }

    }
}
