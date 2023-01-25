﻿using System;

namespace RevitCheckingLevels.Models.Level {
    /// <summary>
    /// Информация об уровне.
    /// </summary>
    internal class LevelInfo {
        /// <summary>
        /// Номер этажа.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Номер уровня.
        /// </summary>
        public int? SubLevel { get; set; }

        /// <summary>
        /// Тип этажа.
        /// </summary>
        public LevelType LevelType { get; set; }

        /// <summary>
        /// Блок уровня.
        /// </summary>
        public BlockType BlockType { get; set; }

        /// <summary>
        /// Начальный номер блока.
        /// </summary>
        public int StartBlock { get; set; }

        /// <summary>
        /// Последний номер блока.
        /// </summary>
        public int FinishBlock { get; set; }

        /// <summary>
        /// Признак равенства начального и последнего номера блока.
        /// </summary>
        public bool IsSameBlockNum => StartBlock == FinishBlock;

        /// <summary>
        /// Отметка уровня.
        /// </summary>
        public double Elevation { get; set; }

        public string GetLevelName() {
            return $"{LevelType?.Name}{Level:D2} этаж";
        }

        public string GetBlockName() {
            return SubLevel == null
                ? GetBlockNum()
                : GetBlockNum() + "." + SubLevel;
        }

        public string GetElevation() {
            return Elevation.ToString("F3", LevelParserImpl.CultureInfo);
        }

        private string GetBlockNum() {
            return IsSameBlockNum
                ? BlockType?.Name + StartBlock
                : BlockType?.Name + StartBlock + GetDelimiter() + BlockType?.Name + FinishBlock;
        }

        private string GetDelimiter() {
            return Math.Abs(FinishBlock - StartBlock) > 1
                ? "-"
                : ", ";
        }

        public override string ToString() {
            return $"{GetLevelName()}_{GetBlockName()}_{GetElevation()}";
        }
    }
}