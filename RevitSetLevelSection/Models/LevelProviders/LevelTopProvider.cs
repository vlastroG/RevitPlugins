﻿using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

using RevitSetLevelSection.Models.ElementPositions;

namespace RevitSetLevelSection.Models.LevelProviders {
    internal class LevelTopProvider : ILevelProvider {
        private readonly IElementPosition _elementPosition;
        private readonly ILevelElevationService _levelElevationService;

        public LevelTopProvider(IElementPosition elementPosition, ILevelElevationService levelElevationService) {
            _elementPosition = elementPosition;
            _levelElevationService = levelElevationService;
        }

        public Level GetLevel(Element element, List<Level> levels) {
            double position = _elementPosition.GetPosition(element);
            return levels.OrderBy(item => item.Elevation)
                .Where(item => _levelElevationService.GetElevation(item) > position)
                .FirstOrDefault();
        }
    }
}