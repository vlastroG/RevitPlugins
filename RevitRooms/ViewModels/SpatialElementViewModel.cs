﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

using dosymep.Revit;
using dosymep.Bim4Everyone;
using dosymep.Bim4Everyone.ProjectParams;
using dosymep.WPF.ViewModels;

using RevitRooms.Models;
using dosymep.Bim4Everyone.SharedParams;

namespace RevitRooms.ViewModels {
    internal class SpatialElementViewModel : ElementViewModel<SpatialElement> {
        public SpatialElementViewModel(SpatialElement element, RevitRepository revitRepository)
            : base(element, revitRepository) {
            var phase = revitRepository.GetPhase(element);
            if(phase != null) {
                Phase = new PhaseViewModel(phase, revitRepository);
            }

            if(RoomArea == null || RoomArea == 0) {
                var segments = Element.GetBoundarySegments(new SpatialElementBoundaryOptions());
                IsRedundant = segments.Count > 0;
                NotEnclosed = segments.Count == 0;

                var boundarySegment = segments.FirstOrDefault();
                IsCountourIntersect = GetCountourIntersect(boundarySegment);
            }
        }

        public Element RoomTypeGroup {
            get { return GetParamElement(ProjectParamsConfig.Instance.RoomTypeGroupName); }
        }

        public Element Room {
            get { return GetParamElement(ProjectParamsConfig.Instance.RoomName); }
        }

        public Element RoomGroup {
            get { return GetParamElement(ProjectParamsConfig.Instance.RoomGroupName); }
        }

        public Element RoomSection {
            get { return GetParamElement(ProjectParamsConfig.Instance.RoomSectionName); }
        }

        public ElementId LevelId {
            get { return Element.LevelId; }
        }

        public string LevelName {
            get { return Element.Level.Name; }
        }

        public double? RoomArea {
            get { return (double?) Element.GetParamValueOrDefault(BuiltInParameter.ROOM_AREA); }
        }

        public bool? IsRoomLiving {
            get { return Convert.ToInt32(Element.GetParamValueOrDefault(ProjectParamsConfig.Instance.IsRoomLiving)) == 1; }
        }

        public bool? IsRoomBalcony {
            get { return Convert.ToInt32(Element.GetParamValueOrDefault(ProjectParamsConfig.Instance.IsRoomBalcony)) == 1; }
        }

        public double? RoomAreaRatio {
            get { return (double?) Element.GetParamValueOrDefault(ProjectParamsConfig.Instance.RoomAreaRatio); }
        }

        public double? Area {
            get { return (double?) Element.GetParamValueOrDefault(SharedParamsConfig.Instance.RoomArea); }
            set { Element.SetParamValue(SharedParamsConfig.Instance.RoomArea, value ?? 0); }
        }

        public double? AreaWithRatio {
            get { return (double?) Element.GetParamValueOrDefault(SharedParamsConfig.Instance.RoomAreaWithRatio); }
            set { Element.SetParamValue(SharedParamsConfig.Instance.RoomAreaWithRatio, value ?? 0); }
        }

        public double ComputeRoomAreaWithRatio() {
            // RoomArea = 0 - по умолчанию
            // RoomAreaRatio = 1 - по умолчанию
            return ((RoomAreaRatio ?? 0) == 0 ? 1 : RoomAreaRatio.Value) * RoomArea ?? 0;
        }

        public bool IsPlaced {
            get { return Element.Location != null; }
        }

        public bool? IsRedundant { get; }
        public bool? NotEnclosed { get; }
        public bool? IsCountourIntersect { get; }

        public PhaseViewModel Phase { get; }

        public void UpdateSharedParams() {
            Element.SetParamValue(SharedParamsConfig.Instance.ApartmentAreaSpec, ProjectParamsConfig.Instance.ApartmentAreaSpec);
            Element.SetParamValue(SharedParamsConfig.Instance.ApartmentAreaMinSpec, ProjectParamsConfig.Instance.ApartmentAreaMinSpec);
            Element.SetParamValue(SharedParamsConfig.Instance.ApartmentAreaMaxSpec, ProjectParamsConfig.Instance.ApartmentAreaMaxSpec);

            Element.SetParamValue(SharedParamsConfig.Instance.RoomGroupShortName, ProjectParamsConfig.Instance.RoomGroupShortName);
            Element.SetParamValue(SharedParamsConfig.Instance.RoomSectionShortName, ProjectParamsConfig.Instance.RoomSectionShortName);
            Element.SetParamValue(SharedParamsConfig.Instance.RoomTypeGroupShortName, ProjectParamsConfig.Instance.RoomTypeGroupShortName);
            Element.SetParamValue(SharedParamsConfig.Instance.FireCompartmentShortName, ProjectParamsConfig.Instance.FireCompartmentShortName);
        }

        public void UpdateLevelSharedParam() {
            Element.SetParamValue(SharedParamsConfig.Instance.Level, Element.Level.Name.Replace(" этаж", string.Empty));
        }

        private Element GetParamElement(RevitParam revitParam) {
            ElementId elementId = (ElementId) Element.GetParamValueOrDefault(revitParam);
            return elementId == null ? null : Element.Document.GetElement(elementId);
        }

        private bool? GetCountourIntersect(IList<BoundarySegment> boundarySegment) {
            if(boundarySegment == null) {
                return null;
            }

            var curves = boundarySegment.Select(item => item.GetCurve());
            var array = new IntersectionResultArray();
            foreach(var curve1 in curves) {
                foreach(var curve2 in curves) {
                    if(curve1.Intersect(curve2) == SetComparisonResult.Overlap) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}