using System.Collections.Generic;
using System;
using System.Linq;
using Models.Helpers;
using Models;
using UnityEngine;


namespace Models {
    public class Board : IUpdatable<Board> {
        public event EventHandler Updated;
        public List<Town> towns { protected set; get; }
        public List<Road> roads { protected set; get; }

        public Board(List<Road> roads, List<Town> towns) {
            this.roads = new List<Road>(roads);
            this.towns = new List<Town>(towns);
        }

        public bool Update(Board update) {
            bool modified = false;

            if (towns.DeepUpdate(update.towns)) {
                modified = true;
            }
            
            if (roads.DeepUpdate(update.roads)) {
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }

        public Town GetTown(string name) {
            foreach (Town town in towns) {
                if (town.name == name) {
                    return town;
                }
            }
            
            return null;
        }

        public Road GetRoad(Town start, Town end, TerrainType roadType) {
            Debug.Log("Passed-in start town is: " + start.name + " with id " + start.id + ", passed-in end town is: " + end.name + " with id " + end.id);
            foreach (Road road in roads) {
                Debug.Log("GetRoad loop");
                Debug.Log("CurRoad Start town is: " + road.start.name + " with id " + road.start.id + ", end town is: " + road.end.name + " with id " + road.end.id + " and type is " + road.roadType + ". Equality results are " + road.start.Equals(start) + " and " + road.end.Equals(end));
                if (road.start.Equals(start) &&
                    road.end.Equals(end) &&
                    road.roadType == roadType) {
                    return road;
                }
            }
            Debug.Log("Road not found in board!");
            return null;
        }
    }
}