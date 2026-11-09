using LongRoad.Misc;
using System.Linq;

namespace LongRoad.Managers
{
    public class BackgroundManager : LongRoadBehaviour
    {
        private InfiniteSpriteScroll[] scrolls;

        public bool IsAnyEasing => scrolls.Any(i => i.IsEasing);
 
        public override void Init()
        {
            scrolls = GetComponentsInChildren<InfiniteSpriteScroll>();

            foreach (var scroll in scrolls)
            {
                scroll.Init();
            }
        }

        public void StartScroll(float easeTime, float speedFactor = 1)
        {
            foreach (var scroll in scrolls)
            {
                scroll.StartScroll(easeTime, speedFactor);
            }
        }

        public void StopScroll(float easeTime)
        {
            foreach (var scroll in scrolls)
            {
                scroll.StopScroll(easeTime);
            }
        }

        public override void Dispose()
        {
            foreach (var item in scrolls)
            {
                item.Dispose();
            }
        }
    }
}
