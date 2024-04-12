namespace AlgorithmsLibrary
{
    public static class AlgmFabrics
    {
        public static ISimplificationAlgm GetAlgmByNameAndParam(string algmName, bool isPercent)
        {
            ISimplificationAlgm? algm = null;
            switch (algmName)
            {
                case "DouglasPeucker":
                    if (isPercent)
                    {
                        algm = new DouglasPeuckerAlgmWithCriterion( new PointPercentCriterion());
                    }
                    else algm = new DouglasPeuckerAlgm();
                    break;
                case "VisvWhyatt": if (isPercent ) algm= new VisWhyattAlgmWithPercent();
                                       else algm= new VisWhyattAlgmWithTolerance();
                    break;
                case "SleeveFit":
                    if (isPercent) algm = new SleeveFitWithCriterion(new PointPercentCriterion());
                    else algm = new SleeveFitAlgm();
                    break;
            }
            return algm;

        }
    }
}
