using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Controller.Scene
{
    public class ScenePlaceGrid : BaseController
    {
        private List<PlaceGridController> _placeGridControllers = new List<PlaceGridController>();
        
        public ScenePlaceGrid(GameObject gameObject) : base(gameObject)
        {
            GameObject goPlaceGrid = Utils.AddChild(gameObject, "place_grid_1");
            PlaceGridController placeGridController = new PlaceGridController(goPlaceGrid);
            _placeGridControllers.Add(placeGridController);
        }

        public override void Dispose()
        {
            foreach (var placeGridController in _placeGridControllers)
            {
                placeGridController.Dispose();
            }
            base.Dispose();
        }

        [CanBeNull]
        public PlaceGridController GetPlaceGridController(Vector3 position)
        {
            foreach (var placeGridController in _placeGridControllers)
            {
                if (placeGridController.IsPositionInside(position))
                {
                    return placeGridController;
                }
            }

            return null;
        }
    }
}