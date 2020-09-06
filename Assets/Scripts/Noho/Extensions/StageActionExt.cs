using Noho.Parsing.Models;

namespace Noho.Extensions
{
    public static class StageActionExt
    {
        public static int[] CleanGunkIndices(this StageAction stageAction)
        {
            int[] indices;
            if (stageAction.GunkIndices == null)
            {
                // default indices
                indices = new[]
                {
                    1
                };
            }
            else
            {
                indices = stageAction.GunkIndices;
            }

            return indices;
        }
    }
}