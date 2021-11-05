using System.Collections.Generic;
using e23.VehicleController.Examples;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace e23.VehicleController.Editor
{
    public class VehiclePrefabBuilderEditorWindow : EditorWindow
    {
        [HideInInspector] [SerializeField] string vehicleName = "newVehicle";
        [HideInInspector] [SerializeField] GameObject vehicleModel;

        [HideInInspector] [SerializeField] VehicleType vehicleType;

        [HideInInspector] [SerializeField] PhysicMaterial physicsMaterial;
        
        [HideInInspector] [SerializeField] string vehicleBodyName = "body";
        [HideInInspector] [SerializeField] string frontLeftWheelName = "frontWheelLeft";
        [HideInInspector] [SerializeField] string frontRightWheelName = "fronWheelRight";
        [HideInInspector] [SerializeField] string backLeftWheelName = "backWheelLeft";
        [HideInInspector] [SerializeField] string backRightWheelName = "backWheelRight";

        [HideInInspector] [SerializeField] VehicleBehaviourSettings vehicleSettings;
        
        [HideInInspector] [SerializeField] bool addEffectsComponent;
        [HideInInspector] [SerializeField] GameObject smokeParticleSystemPrefab;
        [HideInInspector] [SerializeField] int smokeCount = 1;
        [HideInInspector] [SerializeField] GameObject trailRendererPrefab;
        [HideInInspector] [SerializeField] int trailCount = 0;
        [HideInInspector] [SerializeField] bool addExampleInput;
        [HideInInspector] [SerializeField] List<MonoScript> monoBehaviours = new List<MonoScript>();

        [SerializeField] private VehicleBuilderSettings vehicleBuilderSettings;

        private Vector3 parentOffset = new Vector3(0, 0.75f, 0);                // offset used to position the Parent of the whole vehicle above the ground
        private Quaternion defaultRotation = Quaternion.Euler(0, 0, 0);         // cache the default rotation 
        private Vector3 behaviourOffset = new Vector3(0, 0.35f, 0);             // offset of the vehicle behaviour game object
        private Vector3 physicsSphereCenter = new Vector3(0,0,0);               // var to easily change the physics sphere collider center
        private float physicsSphereRadius = 0.75f;                              // size of the physics sphere collider
        private Vector3 modelOffset = new Vector3(0, -1.1f, 0);                 // offset to place the model at ground level
        private List<Transform> wheelTransforms;                                // list of the wheels

        private ReorderableList reorderableBehavioursList;
        private Vector2 scrollPos;

        [MenuItem("Tools/e23/Vehicle Prefab Builder")]
        private static void Init()
        {
#pragma warning disable 0219
            VehiclePrefabBuilderEditorWindow window = (VehiclePrefabBuilderEditorWindow)GetWindow(typeof(VehiclePrefabBuilderEditorWindow));
#pragma warning restore 0219
        }

        private void OnEnable()
        {
            FindVehicleBuilderSettings();
            SetupReorderable();
        }

        private void OnDisable()
        {
            SavePrefabSetup();
        }

        private void FindVehicleBuilderSettings()
        {
            string settingsType = "t:" + nameof(VehicleBuilderSettings);
            string[] guids = AssetDatabase.FindAssets(settingsType);
            
            if (guids.Length == 0)
            {
                VehicleBuilderSettings newSettings = ScriptableObject.CreateInstance<VehicleBuilderSettings>();
                AssetDatabase.CreateAsset(newSettings, "Assets/e23/ArcadeVehicleController/Scripts/Editor/VehicleBuilderSettings.asset");
                vehicleBuilderSettings = newSettings;

                SavePrefabSetup();
            }

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                VehicleBuilderSettings vbs = (VehicleBuilderSettings)AssetDatabase.LoadAssetAtPath(path, typeof(VehicleBuilderSettings));

                vehicleBuilderSettings = vbs;
                LoadPrefabSetup();
            }
        }

        private void SetupReorderable()
        {
            reorderableBehavioursList = new ReorderableList(monoBehaviours, typeof(MonoScript), true, true, true, true);
            reorderableBehavioursList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = monoBehaviours[index];
                monoBehaviours[index] = (MonoScript)EditorGUI.ObjectField(rect, element, typeof(MonoScript), false);
            };
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Build Vehicle", GUILayout.MinHeight(100), GUILayout.Height(50)))
            {
                CreateVehicle();
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 75f));

            CreateLabelField("Vehicle", true);
            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Vehicle Name");
            vehicleName = EditorGUILayout.TextField(vehicleName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Vehicle Model");
            vehicleModel = (GameObject)EditorGUILayout.ObjectField(vehicleModel, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Vehicle Type");
            vehicleType = (VehicleType)EditorGUILayout.EnumPopup(vehicleType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Physics Material");
            physicsMaterial = (PhysicMaterial)EditorGUILayout.ObjectField(physicsMaterial, typeof(PhysicMaterial), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            CreateLabelField("Body Name", true);
            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Vehicle Body");
            vehicleBodyName = EditorGUILayout.TextField(vehicleBodyName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            CreateLabelField("Wheel Names", true);
            if (vehicleType == VehicleType.FourWheels)
            {
                DisplayTwoWheels();
                DisplayFourWheels();
            }
            else if (vehicleType == VehicleType.TwoWheels)
            {
                DisplayTwoWheels();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Vehicle Settings");
            vehicleSettings = (VehicleBehaviourSettings)EditorGUILayout.ObjectField(vehicleSettings, typeof(VehicleBehaviourSettings), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            CreateLabelField("Optional Components", true);
            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Add effects component");
            addEffectsComponent = EditorGUILayout.Toggle("", addEffectsComponent);
            EditorGUILayout.EndHorizontal();

            if (addEffectsComponent == true)
            {
                DisplayEffects();
            }

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Add example input");
            addExampleInput = EditorGUILayout.Toggle("", addExampleInput);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            CreateLabelField("Custom components", true);
            reorderableBehavioursList.DoLayoutList();

            UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(this);
            editor.DrawDefaultInspector();

            EditorGUILayout.EndScrollView();
        }

        private void DisplayTwoWheels()
        {
            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Front Left Wheel");
            frontLeftWheelName = EditorGUILayout.TextField(frontLeftWheelName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Back Left Wheel");
            backLeftWheelName = EditorGUILayout.TextField(backLeftWheelName);
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayFourWheels()
        {
            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Front Right Wheel");
            frontRightWheelName = EditorGUILayout.TextField(frontRightWheelName);
            EditorGUILayout.EndHorizontal();        

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Back Right Wheel");
            backRightWheelName = EditorGUILayout.TextField(backRightWheelName);
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayEffects()
        {
            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Smoke Particles Prefab");
            smokeParticleSystemPrefab = (GameObject)EditorGUILayout.ObjectField(smokeParticleSystemPrefab, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Trail Renderer Prefab");
            trailRendererPrefab = (GameObject)EditorGUILayout.ObjectField(trailRendererPrefab, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CreateLabelField("Trail Count");
            trailCount = EditorGUILayout.IntSlider(trailCount, 0, 4);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
        }

        private void CreateLabelField(string label, bool bold = false)
        {
            GUIStyle editorStyles = bold ? EditorStyles.boldLabel : EditorStyles.label;
            EditorGUILayout.LabelField(label, editorStyles);
        }

        private void CreateVehicle()
        {
            GameObject vehicleParent = new GameObject(vehicleName);
            UpdateTransform(vehicleParent.transform, null, parentOffset, defaultRotation);

            GameObject vehicleBehaviour = new GameObject("Vehicle");
            SetupVehicleBehaviour(vehicleBehaviour, vehicleParent.transform);

            GameObject sphereRigidBody = new GameObject("SphereRigidBody");
            SetupSphere(sphereRigidBody, vehicleParent.transform);
            Rigidbody srb = sphereRigidBody.GetComponent<Rigidbody>();

            GameObject newVehicleModel = (GameObject) PrefabUtility.InstantiatePrefab(vehicleModel);
            SetupModel(newVehicleModel, vehicleBehaviour.transform);

            VehicleBehaviour vb = vehicleBehaviour.GetComponent<VehicleBehaviour>();
            TryFindVehicleParts(vb, newVehicleModel, srb);

            if (addEffectsComponent == true)
            {
                GetWheelTransforms(vb);
                CreateEffectsObjects(vehicleBehaviour.transform);
            }

            AddCustomComponents(vehicleParent.transform);
            
            Selection.activeObject = vehicleBehaviour;
        }

        private void SetupVehicleBehaviour(GameObject obj, Transform parent)
        {
            UpdateTransform(obj.transform, parent, behaviourOffset, defaultRotation);

            obj.AddComponent<VehicleBehaviour>();
            
            if (addEffectsComponent == true)
            {
                obj.AddComponent<VehicleEffects>();
            }

            if (addExampleInput == true)
            {
                ExampleInput exInput = obj.AddComponent<ExampleInput>();
                exInput.VehicleBehaviour = obj.GetComponent<VehicleBehaviour>();
            }
        }

        private void SetupSphere(GameObject obj, Transform parent)
        {
            UpdateTransform(obj.transform, parent, Vector3.zero, defaultRotation);

            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.mass = 100;
            rb.drag = 1;
            rb.angularDrag = 0;

            VehicleBehaviour vb = parent.GetComponentInChildren<VehicleBehaviour>();
            vb.PhysicsSphere = rb;

            SphereCollider sc = obj.AddComponent<SphereCollider>();
            SetupSphereCollider(sc, physicsSphereCenter, physicsSphereRadius, false, physicsMaterial);
        }

        private void SetupSphereCollider(SphereCollider sc, Vector3 center, float radius, bool isTrigger, PhysicMaterial phxMaterial = null)
        {
            sc.center = center;
            sc.radius = radius;
            sc.isTrigger = isTrigger;
            sc.material = phxMaterial;
        }

        private void SetupModel(GameObject obj, Transform parent)
        {
            if (obj == null)
            {
                Debug.Log("No vehicle model prefab has been assigned, only the skeleton will be created");
                return;
            }

            obj.name = vehicleModel.name;
            UpdateTransform(obj.transform, parent, modelOffset, defaultRotation);
        }

        private void TryFindVehicleParts(VehicleBehaviour vehicleBehaviour, GameObject obj, Rigidbody sphereRigidBody)
        {
            if (obj == null)
            {
                return;
            }

            vehicleBehaviour.VehicleModel = obj.transform;
            vehicleBehaviour.PhysicsSphere = sphereRigidBody;
            vehicleBehaviour.VehicleBody = SearchForPart(vehicleBehaviour.transform, vehicleBodyName);
            
            vehicleBehaviour.VehicleWheelCount = vehicleType;
            
            vehicleBehaviour.FrontLeftWheel = SearchForPart(vehicleBehaviour.transform, frontLeftWheelName);
            vehicleBehaviour.FrontRightWheel = SearchForPart(vehicleBehaviour.transform, frontRightWheelName);
            vehicleBehaviour.BackLeftWheel = SearchForPart(vehicleBehaviour.transform, backLeftWheelName);
            vehicleBehaviour.BackRightWheel = SearchForPart(vehicleBehaviour.transform, backRightWheelName);

            if (vehicleSettings != null)
            {
                vehicleBehaviour.VehicleSettings = vehicleSettings;
            }
            else
            {
                Debug.Log("No Vehicle Settings have been added, to create one: Right mouse click in the project window -> create -> e23 -> Vehicle Settings. Then assign the asset.");
            }
        }

        private Transform SearchForPart(Transform parent, string part)
        {
            foreach(Transform t in parent.GetComponentsInChildren<Transform>()){

                string name = t.name.ToLower();
                if (name.Contains(part.ToLower()))
                {
                    return t;
                }
            }
            
            return null;
        }

        private void AddCustomComponents(Transform parent)
        {
            if (monoBehaviours.Count == 0)
            {
                return;
            }

            GameObject customComponents = new GameObject("CustomComponents");
            UpdateTransform(customComponents.transform, parent, Vector3.zero, defaultRotation);

            for (int i = 0; i < monoBehaviours.Count; i++)
            {
                var customClass = monoBehaviours[i].GetClass();
                customComponents.AddComponent(customClass);
            }
        }

        private void CreateEffectsObjects(Transform parent)
        {
            if (smokeParticleSystemPrefab != null)
            {
                for (int i = 0; i < smokeCount; i++)
                {
                    GameObject newSmokeParticles = (GameObject) PrefabUtility.InstantiatePrefab(smokeParticleSystemPrefab);
                    newSmokeParticles.name = smokeParticleSystemPrefab.name;
                    UpdateTransform(newSmokeParticles.transform, parent, behaviourOffset, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                }
            }

            if (trailRendererPrefab != null && trailCount > 0)
            {
                GameObject trailParent = new GameObject("TrailsParent");
                UpdateTransform(trailParent.transform, parent, Vector3.zero, defaultRotation);

                for (int i = trailCount - 1; i >=0; i--)
                {
                    GameObject newtrailRenderer = (GameObject) PrefabUtility.InstantiatePrefab(trailRendererPrefab);
                    newtrailRenderer.name = trailRendererPrefab.name;
                    Vector3 trailPos = wheelTransforms[i] != null ? wheelTransforms[i].position : Vector3.zero;
                    trailPos.y = modelOffset.y + 0.03f;
                    UpdateTransform(newtrailRenderer.transform, trailParent.transform, trailPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
                }
            }       
        }

        private void UpdateTransform(Transform obj, Transform parent, Vector3 pos, Quaternion rot)
        {
            obj.parent = parent;
            obj.localPosition = pos;
            obj.localRotation = rot;
        }

        private void GetWheelTransforms(VehicleBehaviour vehicleBehaviour)
        {
            wheelTransforms = new List<Transform>();

            if (vehicleBehaviour.BackLeftWheel != null) { wheelTransforms.Add(vehicleBehaviour.BackLeftWheel); }
            if (vehicleBehaviour.BackRightWheel != null) { wheelTransforms.Add(vehicleBehaviour.BackRightWheel); }
            if (vehicleBehaviour.FrontLeftWheel != null) { wheelTransforms.Add(vehicleBehaviour.FrontLeftWheel); }
            if (vehicleBehaviour.FrontRightWheel != null) { wheelTransforms.Add(vehicleBehaviour.FrontRightWheel); }
        }

        private void LoadPrefabSetup()
        {
            vehicleName = vehicleBuilderSettings.VehicleName;
            vehicleModel = vehicleBuilderSettings.VehicleModel;
            vehicleType = vehicleBuilderSettings.VehicleType;
            physicsMaterial = vehicleBuilderSettings.PhysicsMaterial;
            vehicleBodyName = vehicleBuilderSettings.BodyName;
            frontLeftWheelName = vehicleBuilderSettings.FrontLeftWheelName;
            frontRightWheelName = vehicleBuilderSettings.FrontRightWheelName;
            backLeftWheelName = vehicleBuilderSettings.BackLeftWheelName;
            backRightWheelName = vehicleBuilderSettings.BackRightWheelName;
            vehicleSettings = vehicleBuilderSettings.VechicleSettings;
            addEffectsComponent = vehicleBuilderSettings.AddEffectsComponent;
            smokeParticleSystemPrefab = vehicleBuilderSettings.SmokeParticleSystemPrefab;
            smokeCount = vehicleBuilderSettings.SmokeCount;
            trailRendererPrefab = vehicleBuilderSettings.TrailRendererPrefab;
            trailCount = vehicleBuilderSettings.TrailCount;
            addExampleInput = vehicleBuilderSettings.AddExampleInput;
            monoBehaviours = vehicleBuilderSettings.MonoBehaviours;
        }

        private void SavePrefabSetup()
        {
            vehicleBuilderSettings.VehicleName = vehicleName;
            vehicleBuilderSettings.VehicleModel = vehicleModel;
            vehicleBuilderSettings.VehicleType = vehicleType;
            vehicleBuilderSettings.PhysicsMaterial = physicsMaterial;
            vehicleBuilderSettings.BodyName = vehicleBodyName;
            vehicleBuilderSettings.FrontLeftWheelName = frontLeftWheelName;
            vehicleBuilderSettings.FrontRightWheelName = frontRightWheelName;
            vehicleBuilderSettings.BackLeftWheelName = backLeftWheelName;
            vehicleBuilderSettings.BackRightWheelName = backRightWheelName;
            vehicleBuilderSettings.VechicleSettings = vehicleSettings;
            vehicleBuilderSettings.AddEffectsComponent = addEffectsComponent;
            vehicleBuilderSettings.SmokeParticleSystemPrefab = smokeParticleSystemPrefab;
            vehicleBuilderSettings.SmokeCount = smokeCount;
            vehicleBuilderSettings.TrailRendererPrefab = trailRendererPrefab;
            vehicleBuilderSettings.TrailCount = trailCount;
            vehicleBuilderSettings.AddExampleInput = addExampleInput;
            vehicleBuilderSettings.MonoBehaviours = monoBehaviours;

            if (vehicleBuilderSettings != null)
            {
                EditorUtility.SetDirty(vehicleBuilderSettings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}