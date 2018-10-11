using UnityEngine;
using System.Collections.Generic;
using System;

namespace MalbersAnimations.Utilities
{

    public class ActiveMeshes : MonoBehaviour
    {
        [SerializeField]
        public List<ActiveSMesh> Meshes = new List<ActiveSMesh>();
        [HideInInspector]
        [SerializeField]
        public bool showMeshesList = true;

        public virtual void ChangeMesh(int index)
        {
            Meshes[index].ChangeMesh();
        }

        public virtual void ChangeMesh(int indexList, int IndexMesh)
        {
            if (indexList < 0) indexList = 0;
            indexList = indexList % Meshes.Count;

            if (Meshes[indexList] != null) Meshes[indexList].ChangeMesh(IndexMesh);
        }


        public virtual void ChangeMesh(int index, bool next)
        {
            Meshes[index].ChangeMesh(next);
        }

        /// <summary>
        /// Toogle all meshes on the list
        /// </summary>
        public virtual void ChangeMesh(bool next = true)
        {
            foreach (var mesh in Meshes)
            {
                mesh.ChangeMesh(next);
            }
        }

        /// <summary>
        /// Get the Active mesh by their name
        /// </summary>
        public virtual ActiveSMesh GetActiveMesh(string name)
        {
            if (Meshes.Count == 0) return null;

            return Meshes.Find(item => item.Name == name);
        }

        /// <summary>
        /// Get the Active Mesh by their Index
        /// </summary>
        public virtual ActiveSMesh GetActiveMesh(int index)
        {
            if (Meshes.Count == 0) return null;

            if (index >= Meshes.Count) index = 0;
            if (index < 0) index = Meshes.Count - 1;

            return Meshes[index];
        }
    }


    [Serializable]
    public class ActiveSMesh
    {
        [HideInInspector]
        public string Name = "NameHere";
        public Transform[] meshes;
        [HideInInspector]

        [SerializeField] public int Current;


        /// <summary>
        /// Change mesh to the Next/Before
        /// </summary>
        public virtual void ChangeMesh(bool next = true)
        {
            if (next)
                Current++;
            else
                Current--;

            if (Current >= meshes.Length) Current = 0;
            if (Current < 0) Current = meshes.Length - 1;

            foreach (var item in meshes)
            {
                if (item) item.gameObject.SetActive(false);
            }

            if (meshes[Current])
                meshes[Current].gameObject.SetActive(true);
        }

        /// <summary>
        /// Set a mesh by Index
        /// </summary>
        public virtual void ChangeMesh(int Index)
        {
            Current = Index - 1;
            ChangeMesh();
        }
    }
}