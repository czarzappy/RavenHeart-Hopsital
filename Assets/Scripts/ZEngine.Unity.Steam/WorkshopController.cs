using Steamworks.Ugc;
using UnityEngine;

namespace ZEngine.Unity.Steam
{
    public class WorkshopController : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            ZBug.Info("WORKSHOP", "Start");
            
            var result = await Query.Screenshots.GetPageAsync(1);
        
            ZBug.Info("WORKSHOP", result);

            if (result.HasValue)
            {
                var page = result.Value;
                
                ZBug.Info("WORKSHOP", $"Result Count: {page.ResultCount}");
                ZBug.Info("WORKSHOP", $"Total Count: {page.TotalCount}");
                ZBug.Info("WORKSHOP", $"Cached: {page.CachedData}");
                
                foreach (Item entry in result.Value.Entries)
                {
                    ZBug.Info("WORKSHOP", entry);
                }
            }
            else
            {
                ZBug.Info("WORKSHOP", "No screenshot page results");
            }

            var itemResults = await Query.Items.GetPageAsync(1);
            
            ZBug.Info("WORKSHOP", itemResults);

            if (itemResults.HasValue)
            {
                foreach (Item entry in itemResults.Value.Entries)
                {
                    ZBug.Info("WORKSHOP", entry);
                }
            }
            else
            {
                ZBug.Info("WORKSHOP", "No item page results");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
