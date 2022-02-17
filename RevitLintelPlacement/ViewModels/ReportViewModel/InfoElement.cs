﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitLintelPlacement.ViewModels {
    internal class InfoElement {

        public static InfoElement LintelIsFixedWithoutElement => new InfoElement() { 
            Message = "Под зафиксиованной перемычкой отсутствует проем.",
            TypeInfo = TypeInfo.Warning,
            ElementType = ElementType.Lintel
        };

        public static InfoElement LintelGeometricalDisplaced => new InfoElement() {
            Message = "Перемычка смещена от проема.",
            TypeInfo = TypeInfo.Warning,
            ElementType = ElementType.Lintel
        };

        public static InfoElement MissingLintelParameter => new InfoElement() {
            Message = "У семейства \"{0}\" перемычки отсутсвует параметр \"{1}\".",
            TypeInfo = TypeInfo.Error,
            ElementType = ElementType.Lintel
        };

        public static InfoElement MissingOpeningParameter => new InfoElement() {
            Message = "У элемента \"{0}\" отсутсвует параметр \"{1}\".",
            TypeInfo = TypeInfo.Error,
            ElementType = ElementType.Opening
        };

        public static InfoElement UnsetLintelParamter => new InfoElement() {
            Message = "У перемычки \"{0}\" не удалось установить значение параметра \"{1}\".",
            TypeInfo = TypeInfo.Error,
            ElementType = ElementType.Opening
        };

        public static InfoElement BlankParamter => new InfoElement() {
            Message = "В настройках не установлено значение параметра \"{0}\".",
            TypeInfo = TypeInfo.Error,
            ElementType = ElementType.Config
        };

        public string Message { get; set; }
        public TypeInfo TypeInfo { get; set; }
        public ElementType ElementType { get; set; }
        public InfoElement FormatMessage(params string[] args) {
            return new InfoElement() {
                TypeInfo = TypeInfo,
                Message = string.Format(Message, args),
                ElementType = ElementType
            };
        }
    }
}
