﻿using System;

using RevitOpeningPlacement.Models.Interfaces;
using RevitOpeningPlacement.Models.OpeningPlacement.AngleFinders;
using RevitOpeningPlacement.OpeningModels;

namespace RevitOpeningPlacement.Models.RealOpeningKrPlacement.AngleFinders {
    /// <summary>
    /// Класс, предоставляющий угол поворота для размещаемого отверстия КР 
    /// по одному прямоугольному входящему заданию от АР на отверстие в перекрытии в горизонтальной плоскости в единицах ревита
    /// </summary>
    internal class SingleRectangleOpeningArTaskInFloorAngleFinder : IAngleFinder {
        private readonly OpeningArTaskIncoming _incomingTask;


        /// <summary>
        /// Конструктор класса, предоставляющего угол поворота для отверстия КР, размещаемого по заданию от АР на прямоугольное отверстие в перекрытии
        /// </summary>
        /// <param name="incomingTask"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SingleRectangleOpeningArTaskInFloorAngleFinder(OpeningArTaskIncoming incomingTask) {
            _incomingTask = incomingTask ?? throw new ArgumentNullException(nameof(incomingTask));
        }


        public Rotates GetAngle() {
            return new Rotates(0, 0, _incomingTask.Rotation);
        }
    }
}
