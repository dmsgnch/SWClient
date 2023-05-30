using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Resourses.MainGame
{
    public static class HUD_values
    {
        // Определение делегата события
        public delegate void ValuesChangedEventHandler();

        // Событие, вызываемое при изменении значений
        public static event ValuesChangedEventHandler OnValuesChanged;

        // Переменные HUD_values

        private static int _totalNumResourses = 34524;
        public static int totalNumResourses
        {
            get { return _totalNumResourses; }
            set
            {
                _totalNumResourses = value;
                OnValuesChanged?.Invoke();
            }
        }

        private static int _totalNumSoldiers = 3242;
        public static int totalNumSoldiers
        {
            get { return _totalNumSoldiers; }
            set
            {
                _totalNumSoldiers = value;
                OnValuesChanged?.Invoke();
            }
        }
        private static int _totalNumResearchShips = 4564;
        public static int totalNumResearchShips
        {
            get { return _totalNumResearchShips; }
            set
            {
                _totalNumResearchShips = value;
                OnValuesChanged?.Invoke();
            }
        }

        private static int _totalNumColonizationShips = 76547;
        public static int totalNumColonizationShips
        {
            get { return _totalNumColonizationShips; }
            set
            {
                _totalNumColonizationShips = value;
                OnValuesChanged?.Invoke();
            }
        }
        private static int _usedNumSoldiers = 7935;
        public static int usedNumSoldiers
        {
            get { return _usedNumSoldiers; }
            set
            {
                _usedNumSoldiers = value;
                OnValuesChanged?.Invoke();
            }
        }
        private static int _usedNumColonizationShips = 34563;
        public static int usedNumColonizationShips
        {
            get { return _usedNumColonizationShips; }
            set
            {
                _usedNumColonizationShips = value;
                OnValuesChanged?.Invoke();
            }
        }
        private static int _usedNumResearchShips = 45675;
        public static int usedNumResearchShips
        {
            get { return _usedNumResearchShips; }
            set
            {
                _usedNumResearchShips = value;
                OnValuesChanged?.Invoke();
            }
        }
        private static int _timeLeft = 45;
        public static int timeLeft
        {
            get { return _timeLeft; }
            set
            {
                _timeLeft = value;
                OnValuesChanged?.Invoke();
            }
        }
        private static string _currentTurnHeroName = "SuperCum";
        public static string currentTurnHeroName
        {
            get { return _currentTurnHeroName; }
            set
            {
                _currentTurnHeroName = value;
                OnValuesChanged?.Invoke();
            }
        }
    }
}