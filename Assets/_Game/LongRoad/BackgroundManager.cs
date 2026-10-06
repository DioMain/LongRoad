using LongRoad.Misc;
using System.Linq;

namespace LongRoad
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

        public void StartScroll(float speedFactor = 1)
        {
            foreach (var scroll in scrolls)
            {
                scroll.StartScroll(speedFactor);
            }
        }

        public void StopScroll(float speedFactor = 1)
        {
            foreach (var scroll in scrolls)
            {
                scroll.StopScroll();
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
