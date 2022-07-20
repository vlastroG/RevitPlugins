﻿using System.Collections.Generic;
using System.Linq;

using dosymep.Revit;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using System;

namespace RevitOpeningPlacement.Models {
    internal class RevitRepository {
        private readonly Application _application;
        private readonly UIApplication _uiApplication;

        private readonly Document _document;
        private readonly UIDocument _uiDocument;

        public RevitRepository(Application application, Document document) {

            _application = application;
            _uiApplication = new UIApplication(application);

            _document = document;
            _uiDocument = new UIDocument(document);

            UIApplication = _uiApplication;
        }

        public UIApplication UIApplication { get; }

        public static Dictionary<CategoryEnum, string> CategoryNames => new Dictionary<CategoryEnum, string> {
            {CategoryEnum.Pipe, "Трубы" },
            {CategoryEnum.RectangleDuct, "Воздуховоды (прямоугольное сечение)" },
            {CategoryEnum.RoundDuct, "Воздуховоды (круглое сечение)" },
            {CategoryEnum.CableTray, "Лотки" },
            {CategoryEnum.Wall, "Стены" },
            {CategoryEnum.Floor, "Перекрытия" }
        };

        public static Dictionary<Parameters, string> ParameterNames => new Dictionary<Parameters, string>() {
            {Parameters.Diameter, "Диаметр" },
            {Parameters.Height, "Высота" },
            {Parameters.Width, "Ширина" }
        };

        public static Dictionary<OpeningType, string> FamilyName => new Dictionary<OpeningType, string>() {
            {OpeningType.FloorRectangle, "ОбщМд_Отв_Отверстие_Прямоугольное_В перекрытии" },
            {OpeningType.FloorRound, "ОбщМд_Отв_Отверстие_Круглое_В перекрытии" },
            {OpeningType.WallRectangle, "ОбщМд_Отв_Отверстие_Прямоугольное_В стене" },
            {OpeningType.WallRound, "ОбщМд_Отв_Отверстие_Круглое_В стене" },
        };

        public static Dictionary<OpeningType, string> TypeName => new Dictionary<OpeningType, string>() {
            {OpeningType.FloorRectangle, "Прямоугольное" },
            {OpeningType.FloorRound, "Круглое" },
            {OpeningType.WallRectangle, "Прямоугольное" },
            {OpeningType.WallRound, "Круглое" },
        };

        public static string OpeningDiameter => "ADSK_Размер_Диаметр";
        public static string OpeningThickness => "ADSK_Размер_Глубина";
        public static List<BuiltInParameter> MepCurveDiameters => new List<BuiltInParameter>() {
            BuiltInParameter.RBS_PIPE_DIAMETER_PARAM,
            BuiltInParameter.RBS_CURVE_DIAMETER_PARAM
        };

        public FamilySymbol GetOpeningType(OpeningType type) {
            return new FilteredElementCollector(_document)
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .WhereElementIsElementType()
                .OfType<FamilySymbol>()
                .FirstOrDefault(item => item.Name.Equals(TypeName[type]) && item.FamilyName.Equals(FamilyName[type]));
        }

        public Transaction GetTransaction(string transactionName) {
            return _document.StartTransaction(transactionName);
        }

        public Level GetLevel(string name) {
            return new FilteredElementCollector(_document)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .FirstOrDefault(item => item.Name.Equals(name, StringComparison.CurrentCulture));
        }

        public FamilyInstance CreateInstance(FamilySymbol type, XYZ point, Level level) {
            if(level != null) {
                if(!type.IsActive) {
                    type.Activate();
                }
                point = point - XYZ.BasisZ * level.Elevation;
                return _document.Create.NewFamilyInstance(point, type, level, StructuralType.NonStructural);
            }
            return _document.Create.NewFamilyInstance(point, type, StructuralType.NonStructural);
        }

        public void RotateElement(Element element, XYZ point, double angle) {
            if(point != null) {
                ElementTransformUtils.RotateElement(_document, element.Id, Line.CreateBound(point, new XYZ(point.X, point.Y, point.Z + 1)), angle);
            }
        }
    }


    internal enum Parameters {
        Height,
        Width,
        Diameter
    }

    internal enum CategoryEnum {
        Pipe,
        RectangleDuct,
        RoundDuct,
        CableTray,
        Wall,
        Floor
    }

    internal enum OpeningType {
        WallRound,
        WallRectangle,
        FloorRound,
        FloorRectangle
    }
}