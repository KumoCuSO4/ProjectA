using UnityEngine;

namespace Controller.Placeable
{
    public class BaseCashier : BasePlaceable
    {
        public BaseCashier(GameObject gameObject, int tableID, PlaceGridController placeGrid) : base(gameObject, tableID, placeGrid)
        {
        }
    }
}