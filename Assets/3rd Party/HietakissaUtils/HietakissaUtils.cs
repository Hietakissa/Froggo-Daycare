namespace HietakissaUtils
{
    using Random = UnityEngine.Random;
    using System.Collections.Generic;
    //using Newtonsoft.Json;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using System.IO;
    using System;

    public static class Extensions
    {
        public static Vector2 Abs(this Vector2 vector) => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        public static Vector3 Abs(this Vector3 vector) => new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));

        public static Vector2 Round(this Vector2 vector, bool roundUp = false) => roundUp ? new Vector2((float)Math.Ceiling(vector.x), (float)Math.Ceiling(vector.y)) : new Vector2((float)Math.Floor(vector.x), (float)Math.Floor(vector.y));
        public static Vector3 Round(this Vector3 vector, bool roundUp = false) => roundUp ? new Vector3((float)Math.Ceiling(vector.x), (float)Math.Ceiling(vector.y), (float)Math.Ceiling(vector.z)) : new Vector3((float)Math.Floor(vector.x), (float)Math.Floor(vector.y), (float)Math.Floor(vector.z));
        public static Vector2 RoundToNearest(this Vector2 vector) => new Vector2(Maf.RoundToNearest(vector.x), Maf.RoundToNearest(vector.y));
        public static Vector3 RoundToNearest(this Vector3 vector) => new Vector3(Maf.RoundToNearest(vector.x), Maf.RoundToNearest(vector.y), Maf.RoundToNearest(vector.z));
        public static Vector2 RoundToNearest(this Vector2 vector, float roundBy) => new Vector2(vector.x.RoundToNearest(roundBy), vector.y.RoundToNearest(roundBy));
        public static Vector3 RoundToNearest(this Vector3 vector, float roundBy) => new Vector3(vector.x.RoundToNearest(roundBy), vector.y.RoundToNearest(roundBy), vector.z.RoundToNearest(roundBy));

        public static Vector2 SetAxis(this Vector2 vector, string axis = "XY", params float[] values)
        {
            byte usedValues = 0;

            if (axis.Contains("X") && values.Length > usedValues)
            {
                vector.x = values[usedValues];
                usedValues++;
            }
            if (axis.Contains("Y") && values.Length > usedValues)
            {
                vector.y = values[usedValues];
            }

            return vector;
        }
        public static Vector3 SetAxis(this Vector3 vector, string axis = "XYZ", params float[] values)
        {
            byte usedValues = 0;

            if (axis.Contains("X") && values.Length > usedValues)
            {
                vector.x = values[usedValues];
                usedValues++;
            }
            if (axis.Contains("Y") && values.Length > usedValues)
            {
                vector.y = values[usedValues];
                usedValues++;
            }
            if (axis.Contains("Z") && values.Length > usedValues)
            {
                vector.z = values[usedValues];
            }

            return vector;
        }

        public static Vector2 SetX(this Vector2 vector, float value)
        {
            vector.x = value;
            return vector;
        }
        public static Vector2 SetY(this Vector2 vector, float value)
        {
            vector.y = value;
            return vector;
        }

        public static Vector3 SetX(this Vector3 vector, float value)
        {
            vector.x = value;
            return vector;
        }
        public static Vector3 SetY(this Vector3 vector, float value)
        {
            vector.y = value;
            return vector;
        }
        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.z = value;
            return vector;
        }

        public static int Round(this float roundNum, bool roundUp = false) => roundUp ? (int)Math.Ceiling(roundNum) : (int)Math.Floor(roundNum);
        public static int RoundToNearest(this float num) => Maf.RoundToNearest(num);
        public static float RoundToDecimalPlaces(this float num, int decimalPlaces) => Maf.RoundToDecimalPlaces(num, decimalPlaces);
        public static float RoundToNearest(this float num, float roundBy) => Maf.RoundToNearest(num, roundBy);

        public static string AddInFrontOfMatches(this string text, string textToAdd, params string[] matches)
        {
            foreach (string match in matches) text = text.Replace(match, $"{textToAdd}{match}");
            return text;
        }
        public static string ReplaceMultiple(this string text, string replacement, params string[] targets)
        {
            foreach (string target in targets) text.Replace(target, replacement);
            return text;
        }
        public static string ReplaceFirst(this string text, string match, string replacement)
        {
            int pos = text.IndexOf(match);

            if (pos < 0) return text;

            return text.Substring(0, pos) + replacement + text.Substring(pos + match.Length);
        }
        public static string Remove(this string targetString, string stringToRemove)
        {
            return targetString.Replace(stringToRemove, "");
        }
        public static string RemoveFirst(this string targetString, string stringToRemove)
        {
            return targetString.ReplaceFirst(stringToRemove, "");
        }

        public static int Abs(this int absInt)
        {
            return Mathf.Abs(absInt);
        }
        public static float Abs(this float absFloat)
        {
            return Mathf.Abs(absFloat);
        }

        public static float FlipOne(this float num)
        {
            return Maf.FlipOne(num);
        }

        public static Quaternion ToQuaternion(this Vector3 euler)
        {
            return Quaternion.Euler(euler);
        }
        public static Vector3 ToEuler(this Quaternion quaternion)
        {
            return quaternion.eulerAngles;
        }

        public static int Magnitude(this float num)
        {
            return num > 0 ? 1 : (num < 0 ? -1 : 0);
        }

        public static bool IsPositive(this float num) => num >= 0f ? true : false;
        public static bool IsPositive(this int num) => num >= 0 ? true : false;
        public static bool IsNegative(this float num) => num <= 0f ? true : false;
        public static bool IsNegative(this int num) => num <= 0 ? true : false;

        public static bool ToBool(this int num) => num == 1 ? true : false;

        public static bool IndexInBounds<TType>(this TType[] array, int index) => index >= 0 && index < array.Length;
        public static bool IndexInBounds<TType>(this List<TType> array, int index) => index >= 0 && index < array.Count;

        public static TElement RandomElement<TElement>(this TElement[] array) => array[Random.Range(0, array.Length)];
        public static TElement RandomElement<TElement>(this List<TElement> list) => list[Random.Range(0, list.Count)];

        public static bool Contains(this string[] array, string value)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (array[i] == value) return true;
            }
            return false;
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static void DestroyChildren(this Transform transform)
        {
            int childCount = transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
        public static void DestroyChildrenImmediate(this Transform transform)
        {
            int childCount = transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }

    public abstract class Maf
    {
        public static Vector2 Vector2Average(params Vector2[] vectors)
        {
            float totalX = 0f, totalY = 0f;

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector2 vector = vectors[i];

                totalX += vector.x;
                totalY += vector.y;
            }

            return new Vector2(totalX / vectors.Length, totalY / vectors.Length);
        }
        public static Vector3 Vector3Average(params Vector3[] vectors)
        {
            float totalX = 0f, totalY = 0f, totalZ = 0f;

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector3 vector = vectors[i];

                totalX += vector.x;
                totalY += vector.y;
                totalZ += vector.z;
            }

            return new Vector3(totalX / vectors.Length, totalY / vectors.Length, totalZ / vectors.Length);
        }
        public static Vector2 RandomVector2(float minValue, float maxValue, string axis = "XY", float defaultValue = 0f)
        {
            Vector2 randomVector = new Vector2(defaultValue, defaultValue);

            if (axis.Contains("X")) randomVector.x = Random.Range(minValue, maxValue);

            if (axis.Contains("Y")) randomVector.y = Random.Range(minValue, maxValue);

            return randomVector;
        }
        public static Vector3 RandomVector3(float minValue, float maxValue, string axis = "XYZ", float defaultValue = 0f)
        {
            Vector3 randomVector = new Vector3(defaultValue, defaultValue, defaultValue);

            if (axis.Contains("X")) randomVector.x = Random.Range(minValue, maxValue);

            if (axis.Contains("Y")) randomVector.y = Random.Range(minValue, maxValue);

            if (axis.Contains("Z")) randomVector.z = Random.Range(minValue, maxValue);

            return randomVector;
        }
        public static Vector3 Direction(Vector3 from, Vector3 to) => (to - from).normalized;

        public static float Average(params float[] nums)
        {
            float num = 0f;

            for (int i = 0; i < nums.Length; i++)
            {
                num += nums[i];
            }

            return num / nums.Length;
        }
        public static int AverageInt(params float[] nums)
        {
            float num = 0f;

            for (int i = 0; i < nums.Length; i++)
            {
                num += nums[i];
            }

            return RoundToNearest(num / nums.Length);
        }

        public static int RoundToNearest(float num) => (int)Math.Round(num, MidpointRounding.AwayFromZero);
        public static float RoundToNearest(float num, float roundBy)
        {
            float remainder = Mathf.Abs(num % roundBy);

            if (num > 0)
            {
                if (remainder < roundBy / 2f) return num - remainder;
                else return num + (roundBy - remainder);
            }
            else
            {
                if (remainder < roundBy / 2f) return num + remainder;
                else return num - (roundBy - remainder);
            }
        }
        public static float RoundToDecimalPlaces(float num, int decimalPlaces) => (float)Math.Round((decimal)num, decimalPlaces);

        public static float ReMap(float iMin, float iMax, float oMin, float oMax, float value)
        {
            float t = Mathf.InverseLerp(iMin, iMax, value);
            return Mathf.Lerp(oMin, oMax, t);
        }
        public static float FlipOne(float num)
        {
            return Mathf.Abs(1f - num);
        }

        public static Vector3 QuaternionToEuler(Quaternion quaternion)
        {
            return quaternion.eulerAngles;
        }
        public static Quaternion EulerToQuaternion(Vector3 euler) => Quaternion.Euler(euler);
        public static Quaternion EulerToQuaternion(float x, float y, float z) => Quaternion.Euler(x, y, z);

        public static Color Lerp(Color a, Color b, float t) => Color.Lerp(a, b, t);
        public static Color Lerp(Color[] colors, float t)
        {
            t = Mathf.Clamp(t, 0, 1);

            float decimalIndex = t * (colors.Length - 1);
            Color startColor = colors[(int)decimalIndex];

            Color endColor;
            if (t == 1) endColor = colors[colors.Length - 1];
            else endColor = colors[(int)(decimalIndex + 1)];

            return Lerp(startColor, endColor, decimalIndex - (int)decimalIndex);
        }
        public static float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, t);
        public static bool Lerp(bool a, bool b, float t) => t >= 0.5f ? b : a;

        public static bool RandomBool(int percentage) => Random.Range(1, 101) <= percentage;
        public static bool RandomBool(float percentage) => Random.Range(0f, 1f) <= percentage * 0.01f;

        public static Vector3 CalculateCollisionPoint(Vector3 targetPos, Vector3 targetVelocity, Vector3 projectilePos, float projectileSpeed)
        {
            float timeToReachObject = CalculateTimeToCollision(targetPos - projectilePos, targetVelocity, projectileSpeed);

            if (timeToReachObject > 0f) return targetPos + targetVelocity * timeToReachObject;
            else return Vector3.zero;

            float CalculateTimeToCollision(Vector3 relativePosition, Vector3 relativeVelocity, float projectileSpeed)
            {
                float rVel = Vector3.Dot(relativeVelocity, relativeVelocity) - projectileSpeed * projectileSpeed;
                float rAvg = Vector3.Dot(relativeVelocity, relativePosition) * 2f;
                float rPos = Vector3.Dot(relativePosition, relativePosition);

                float disc = rAvg * rAvg - rVel * rPos * 4f;

                if (disc > 0f) return rPos * 2f / (Mathf.Sqrt(disc) - rAvg);
                else return -1f;
            }
        }
    }

    public static class QOL
    {
        public static bool Raycast2D(Vector2 origin, Vector2 direction, out RaycastHit2D hit, float distance)
        {
            return hit = Physics2D.Raycast(origin, direction, distance);
        }
        public static bool Raycast2D(Vector2 origin, Vector2 direction, out RaycastHit2D hit, float distance, int layerMask)
        {
            return hit = Physics2D.Raycast(origin, direction, distance, layerMask);
        }

        public static void CopyToClipboard(string text)
        {
            GUIUtility.systemCopyBuffer = text;
        }
    }

    public static class Serializer
    {
        static string SAVEDATA_FOLDER = Application.dataPath + "/SaveData/";
        const string SAVE_FOLDER_TEMPLATE = "Save_";
        const string FILE_EXTENSION = ".SAVE";

        public static int CurrentSaveID = 1;
        public static bool PrettyPrint = true;

        public static int[] GetSaveIDs()
        {
            string[] saveDirectories = Directory.GetDirectories(SAVEDATA_FOLDER);
            int saveDirectoryCount = saveDirectories.Length;

            int[] saveIDs = new int[saveDirectoryCount];

            for (int i = 0; i < saveDirectoryCount; i++)
            {
                saveIDs[i] = GetSaveIDFromSaveDirectory(saveDirectories[i]);
            }
            return saveIDs;
        }

        public static int GetHighestAvailableSaveID()
        {
            int saveID = 1;

            while (Directory.Exists(GetSaveDirectoryFromID(saveID))) saveID++;
            return saveID;
        }

        public static void Save(string saveData, string saveKey, string subFolder = "")
        {
            string directory = GetDirectoryFromParameters(CurrentSaveID, subFolder);
            string fileName = $"{saveKey}{FILE_EXTENSION}";

            HandleFolderStuff(directory);

            File.WriteAllText(directory + fileName, saveData);
        }

        public static string Load(string loadKey, string subFolder = "")
        {
            string directory = GetDirectoryFromParameters(CurrentSaveID, subFolder);
            string fileName = $"{loadKey}{FILE_EXTENSION}";

            if (!File.Exists(directory + fileName))
            {
                Debug.LogError($"Tried to load data from {directory + fileName}, but it doesn't exist. Returned empty string.");
                return "";
            }
            return File.ReadAllText(directory + fileName);
        }

        //public static void SaveObject<TSaveObject>(TSaveObject saveObject, string saveKey, string subFolder = "") => Save(/*JsonUtility.ToJson(saveObject, PrettyPrint)*/JsonConvert.SerializeObject(saveObject, PrettyPrint ? Formatting.Indented : Formatting.None), saveKey, subFolder);

        //public static TLoadObject LoadObject<TLoadObject>(string loadKey, string subFolder = "") => /*JsonUtility.FromJson<TLoadObject>*/JsonConvert.DeserializeObject<TLoadObject>(Load(loadKey, subFolder));

        public static void ClearCurrentSave()
        {
            string directory = GetSaveDirectoryFromID(CurrentSaveID);

            Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
        }

        public static bool SaveDataExists(string key, string subFolder = "") => File.Exists($"{GetDirectoryFromParameters(CurrentSaveID, subFolder)}{key}{FILE_EXTENSION}");

        static void HandleFolderStuff(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static int GetSaveIDFromSaveDirectory(string directory) => int.Parse(directory.Remove(SAVEDATA_FOLDER + SAVE_FOLDER_TEMPLATE));
        public static string GetSaveDirectoryFromID(int saveID) => $"{SAVEDATA_FOLDER}{SAVE_FOLDER_TEMPLATE}{saveID}";

        static string GetDirectoryFromParameters(int saveID, string subFolder)
        {
            if (subFolder == "") return $"{GetSaveDirectoryFromID(saveID)}/";
            else if (subFolder.StartsWith("global")) return $"{SAVEDATA_FOLDER}/{subFolder}/";
            else return $"{GetSaveDirectoryFromID(saveID)}/{subFolder}/";
        }
    }

    public static class ControlRebinding
    {
        static Dictionary<string, KeyCode> bindings;

        static KeyCode[] validKeycodes;
        static KeyCode keyCode;

        public static bool binding { get; private set; }
        static string bindingKeyName;

        public static event Action OnKeyRebound;

        public static void SetValidKeycodes(bool includeController = false)
        {
            bindings = new Dictionary<string, KeyCode>();
            validKeycodes = Enum.GetValues(typeof(KeyCode))
                .Cast<KeyCode>()
                .Where(k => !includeController ? (int)k < 330 : true)
                .ToArray();
        }

        public static KeyCode GetPressedKey()
        {
            if (!Input.anyKeyDown) return KeyCode.None;

            for (int i = 0; i < validKeycodes.Length; i++)
            {
                keyCode = validKeycodes[i];
                if (Input.GetKeyDown(keyCode)) return keyCode;
            }

            return KeyCode.None;
        }

        public static KeyCode GetKeyWithName(string name)
        {
            return bindings[name];
        }

        public static void StartBinding(string name)
        {
            binding = true;
            bindingKeyName = name;
        }

        public static void HandleBinding()
        {
            KeyCode key = GetPressedKey();

            if (key != KeyCode.None)
            {
                EditBinding(bindingKeyName, key);
                binding = false;
            }
        }

        public static void EditBinding(string name, KeyCode key)
        {
            bindings[name] = key;
            OnKeyRebound?.Invoke();
        }

        public static void SaveBindings()
        {
            //Serializer.SaveObject(bindings, "ControlBindings", "global");
        }

        public static bool BindingsExist()
        {
            return Serializer.SaveDataExists("ControlBindings", "global");
        }

        public static void LoadBindings()
        {
            if (BindingsExist())
            {
                //bindings = Serializer.LoadObject<Dictionary<string, KeyCode>>("ControlBindings", "global");
                OnKeyRebound?.Invoke();
            }
        }
    }

    public static class Regexer
    {
        public static RegexObject Begin()
        {
            return new RegexObject();
        }
        public static string Finish(this RegexObject regex)
        {
            return regex.sb.ToString();
        }

        public class RegexObject
        {
            public StringBuilder sb;

            public RegexObject()
            {
                sb = new StringBuilder();
            }

            public RegexObject Any()
            {
                sb.Append(".");
                return AnyTimes();
            }
            public RegexObject Any(int count)
            {
                sb.Append(".");
                return Times(count);
            }
            public RegexObject Any(int min, int max)
            {
                sb.Append($".");
                return MinMaxTimes(min, max);
            }

            public RegexObject Exact(string text)
            {
                text = text.AddInFrontOfMatches("\\", @"\", @"^", @"$", @".", @"|", @"?", @"*", @"+", @"(", @")", @"[", @"]", @"{", @"}");
                sb.Append($"{text}");
                return this;
            }

            public RegexObject UppercaseCharacter()
            {
                sb.Append("[A-Z]");
                return AnyTimes();
            }
            public RegexObject UppercaseCharacter(int count)
            {
                sb.Append("[A-Z]");
                return Times(count);
            }
            public RegexObject UppercaseCharacter(int min, int max)
            {
                sb.Append("[A-Z]");
                return MinMaxTimes(min, max);
            }

            public RegexObject LowercaseCharacter()
            {
                sb.Append("[a-z]");
                return AnyTimes();
            }
            public RegexObject LowercaseCharacter(int count)
            {
                sb.Append("[a-z]");
                return Times(count);
            }
            public RegexObject LowercaseCharacter(int min, int max)
            {
                sb.Append("[a-z]");
                return MinMaxTimes(min, max);
            }

            public RegexObject UppercaseOrLowercaseCharacter()
            {
                sb.Append("[a-zA-Z]");
                return AnyTimes();
            }
            public RegexObject UppercaseOrLowercaseCharacter(int count)
            {
                sb.Append("[a-zA-Z]");
                return Times(count);
            }
            public RegexObject UppercaseOrLowercaseCharacter(int min, int max)
            {
                sb.Append("[a-zA-Z]");
                return MinMaxTimes(min, max);
            }

            public RegexObject Number()
            {
                sb.Append("[0-9]");
                return AnyTimes();
            }
            public RegexObject Number(int count)
            {
                sb.Append("[0-9]");
                return Times(count);
            }
            public RegexObject Number(int min, int max)
            {
                sb.Append("[0-9]");
                return MinMaxTimes(min, max);
            }

            public RegexObject NonSymbolCharacter()
            {
                sb.Append("[a-zA-Z0-9]");
                return AnyTimes();
            }
            public RegexObject NonSymbolCharacter(int count)
            {
                sb.Append("[a-zA-Z0-9]");
                return Times(count);
            }
            public RegexObject NonSymbolCharacter(int min, int max)
            {
                sb.Append("[a-zA-Z0-9]");
                return MinMaxTimes(min, max);
            }

            public RegexObject Custom(string custom)
            {
                sb.Append(custom);
                return this;
            }

            public RegexObject AnyTimes()
            {
                sb.Append("*");
                return this;
            }
            public RegexObject Times(int count)
            {
                sb.Append($@"{{{count}}}");
                return this;
            }
            public RegexObject MinMaxTimes(int min, int max)
            {
                sb.Append($@"{{{min},{max}}}");
                return this;
            }
            public RegexObject MinTimes(int count)
            {
                sb.Append($@"{{{count},}}");
                return this;
            }

            public RegexObject Start()
            {
                sb.Append("^");
                return this;
            }
            public RegexObject End()
            {
                sb.Append("$");
                return this;
            }

            public static implicit operator string (RegexObject regex)
            {
                return regex.Finish();
            }

            public override string ToString()
            {
                return this.Finish();
            }
        }
    }

    public class Pool
    {
        Queue<GameObject> poolQueue;

        GameObject poolObject;
        Transform parent;

        public int GrowSize
        {
            get => GrowSize;
            set
            {
                GrowSize = Mathf.Clamp(value, 1, 1000);
            }
        }

        public int maxSize
        {
            get => maxSize;
            set
            {
                maxSize = Mathf.Clamp(value, 1, 1000);
            }
        }
        public int currentSize;
        public int objectsAvailable;

        public Pool(GameObject poolObject, Transform parent, int growSize = 10, int maxSize = 1000)
        {
            poolQueue = new Queue<GameObject>();

            this.poolObject = poolObject;
            this.parent = parent;
            this.GrowSize = growSize;
            this.maxSize = maxSize;

            currentSize = 0;
            objectsAvailable = 0;

            GrowPool();
        }


        public GameObject Get()
        {
            if (objectsAvailable == 0) GrowPool();
            if (objectsAvailable == 0) return null;

            GameObject getObject = poolQueue.Dequeue();
            objectsAvailable--;

            getObject.SetActive(true);
            return getObject;
        }
        public GameObject Instantiate(Vector3 position, Quaternion rotation)
        {
            GameObject getObject = Get();

            if (getObject == null) return null;

            getObject.transform.position = position;
            getObject.transform.rotation = rotation;

            return getObject;
        }

        public void ReturnObject(GameObject returnObject)
        {
            returnObject.SetActive(false);
            if (!poolQueue.Contains(returnObject)) poolQueue.Enqueue(returnObject);

            objectsAvailable++;
        }

        public bool GrowPool()
        {
            if (currentSize >= maxSize) return false;

            for (int i = 0; i < GrowSize && currentSize < maxSize; i++)
            {
                GameObject newObject = MonoBehaviour.Instantiate(poolObject, parent.position, Quaternion.identity);
                newObject.transform.parent = parent;

                newObject.SetActive(false);
                poolQueue.Enqueue(newObject);

                currentSize++;
                objectsAvailable++;
            }

            return true;
        }

        public void EmptyPool()
        {
            currentSize = 0;
            objectsAvailable = 0;

            poolQueue.Clear();
        }
    }

    [Serializable]
    public class WeightedBag<TBagItem>
    {
        [SerializeField] List<BagItem> bagItems = new List<BagItem>();

        public int TotalWeight { get; private set; } = 0;


        public void AddItem(TBagItem item, int weight)
        {
            bagItems.Add(new BagItem(item, weight));
            CalculateTotalWeight();
        }

        public TBagItem GetItem()
        {
            if (TotalWeight == 0) CalculateTotalWeight();

            int randomWeight = Random.Range(1, TotalWeight + 1);

            foreach (BagItem bagItem in bagItems)
            {
                randomWeight -= bagItem.weight;

                if (randomWeight <= 0) return bagItem.item;
            }

            return default(TBagItem);
        }

        public void CalculateTotalWeight()
        {
            TotalWeight = 0;

            foreach (BagItem item in bagItems) TotalWeight += item.weight;
        }

        public void ClearAllItems()
        {
            bagItems.Clear();
            TotalWeight = 0;
        }

        [Serializable]
        public class BagItem
        {
            public TBagItem item;
            public int weight;

            public BagItem(TBagItem item, int weight)
            {
                this.item = item;
                this.weight = weight;
            }
        }
    }

    public class Grid2D<TGridObject>
    {
        TGridObject[,] gridArray;

        public Action OnValueChanged;

        int gridWidth, gridHeight;
        int cellSize = 1;
        Vector3 gridOrigin;

        public Grid2D(int width, int height)
        {
            //Initialize Grid with given parameters
            gridArray = new TGridObject[width, height];

            gridWidth = width;
            gridHeight = height;
        }

        public Grid2D(int width, int height, Vector3 origin, int cellSize)
        {
            //Initialize Grid with given parameters
            gridArray = new TGridObject[width, height];

            gridWidth = width;
            gridHeight = height;
            this.cellSize = cellSize;
            this.gridOrigin = origin;
        }

        public int GetWidth()
        {
            return gridWidth; //Return width of the grid
        }

        public int GetHeight()
        {
            return gridHeight; //Return height of the grid
        }

        public TGridObject GetValue(int x, int y)
        {
            //If given grid position is valid -> get its value, else return default value
            if (IsValidPos(x, y)) return gridArray[x - 1, y - 1];
            else return default(TGridObject);
        }

        public TGridObject GetValue(Vector3 worldPosition)
        {
            //Convert world position to grid position
            int x, y;
            GetWorldToGrid(worldPosition, out x, out y);

            //Try to get grid value at converted grid position
            return GetValue(x, y);
        }

        public void SetValue(int x, int y, TGridObject value)
        {
            //If given grid position is valid -> set its value to given value
            if (IsValidPos(x, y))
            {
                gridArray[x - 1, y - 1] = value;
                if (OnValueChanged != null) OnValueChanged(); //Fire OnValueChanged Event
            }
        }

        public void SetValue(Vector3 worldPosition, TGridObject value)
        {
            //Convert world position to grid position
            int x, y;
            GetWorldToGrid(worldPosition, out x, out y);

            //Try to set grid value at converted grid position
            SetValue(x, y, value);
        }

        public void ClearGrid()
        {
            //Loop through all grid positions and set their values to default
            for (int y = 1; y < gridHeight + 1; y++)
            {
                for (int x = 1; x < gridWidth + 1; x++)
                {
                    //Debug.Log($"Clearing cell at {x}, {y}");
                    SetValue(x, y, default(TGridObject));
                }
            }
        }

        void GetWorldToGrid(Vector3 worldPos, out int x, out int y)
        {
            //Convert world position to grid position
            Vector3 realPos = new Vector3(Mathf.Abs(gridOrigin.x), Mathf.Abs(gridOrigin.y), Mathf.Abs(gridOrigin.z)) + worldPos;

            //Round up grid position to stay consistent with given pos and output grid pos
            x = (int)(realPos.x / cellSize + 0.5f);
            y = (int)(realPos.y / cellSize + 0.5f);
        }

        public bool IsValidPos(int x, int y)
        {
            //Check whether the given grid pos is withing the grid bounds
            if (x > 0 && y > 0 && x <= gridWidth && y <= gridHeight) return true;
            else return false;
        }

        public bool IsValidPos(Vector3 worldPos)
        {
            //Convert world position to grid position
            int x, y;
            GetWorldToGrid(worldPos, out x, out y);

            //Check whether the given grid pos is withing the grid bounds
            return IsValidPos(x, y);
        }
    }

    namespace Health
    {
        public class HealthSystem
        {
            //Event variables
            public Action OnHealthChanged;
            public Action OnTookDamage;
            public Action OnDied;
            public Action OnRespawned;

            public bool isAlive { get; private set; }

            //Regen component variable
            HealthRegen regen;

            //Health variables
            public float maxHealth { get; private set; }
            public float currentHealth { get; private set; }

            //All regen variables are optional, by default regen will be disabled and regen will happen every second for 5hp
            public HealthSystem(float maxHealth = 100f)
            {
                //Apply health
                this.maxHealth = maxHealth;
                currentHealth = maxHealth;

                isAlive = true;
            }

            public HealthSystem(float maxHealth = 100f, GameObject regenTarget = null, float regenDelay = 1f, float regenAmount = 5f, bool interruptHealing = false, bool healDecimal = false)
            {
                //Apply health
                this.maxHealth = maxHealth;
                currentHealth = maxHealth;

                isAlive = true;

                if (regenTarget != null) //Is there a target for regen
                {
                    //Setup HealthRegen
                    regen = regenTarget.AddComponent<HealthRegen>();
                    regen.Setup(this, healDecimal);

                    SetRegenDelay(regenDelay);
                    SetRegenAmount(regenAmount);
                }
            }

            //Actions
            public void Damage(float damageAmount)
            {
                if (!isAlive) return; //We are already dead -> don't fire events
                damageAmount = Mathf.Abs(damageAmount);

                currentHealth -= damageAmount;

                HealthChangedEvent(); //Attempt to fire the OnHealthChanged Event
                TookDamageEvent(); //Attempt to fire the OnTookDamage Event
            }

            public void DamageDecimal(float damageDecimal)
            {
                Damage(maxHealth * damageDecimal);
            }

            public void Heal(float healAmount)
            {
                if (currentHealth >= maxHealth) return; //We are already at full HP -> don't fire up event
                healAmount = Mathf.Abs(healAmount);

                currentHealth += healAmount;

                HealthChangedEvent(); //Attempt to fire the OnHealthChanged Event
            }

            public void HealDecimal(float healDecimal)
            {
                Heal(maxHealth * healDecimal);
            }

            public void HealToFull()
            {
                Heal(maxHealth); //Set current health to maximum
            }

            public void Respawn(float healthMultiplier) //0 to respawn with 0 health 1 to respawn with full health
            {
                currentHealth = maxHealth * healthMultiplier;

                RespawnEvent();
            }


            //Get stats
            public float GetHealth()
            {
                return currentHealth; //Return current health
            }

            public float GetHealthDecimal()
            {
                return currentHealth / maxHealth; //Return health as a decimal ranging from 0 (dead) to 1 (full hp)
            }

            public float GetMaxHealth()
            {
                return maxHealth;
            }

            public bool IsAlive()
            {
                return isAlive;
            }


            //Other methods
            void ClampHealth()
            {
                if (currentHealth <= 0f && isAlive)
                {
                    DiedEvent(); //Current health is under 0 -> we died
                }

                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); //Clamp current health between 0 and max health
            }


            //Events
            void HealthChangedEvent()
            {
                ClampHealth(); //Clamp health

                if (OnHealthChanged != null) OnHealthChanged(); //Fire off OnHealthChanged Event if there are subscribers
            }

            void TookDamageEvent()
            {
                if (OnTookDamage != null) OnTookDamage(); //Fire off OnTookDamage Event if there are subscribers
            }

            void DiedEvent()
            {
                isAlive = false; //We are not alive

                if (OnDied != null) OnDied(); //Fire off OnDied Event if there are subscribers
            }

            void RespawnEvent()
            {
                isAlive = true;

                if (OnRespawned != null) OnRespawned();
            }


            //Affect regen
            public void SetRegenDelay(float delay)
            {
                if (regen == null) return;
                regen.SetRegenDelay(delay); //Set the HealthRegen component's RegenDelay if it exists
            }

            public void SetRegenAmount(float amount)
            {
                if (regen == null) return;
                regen.SetRegenAmount(amount); //Set the HealthRegen component's RegenAmount if it exists
            }
        }

        public class HealthRegen : MonoBehaviour
        {
            //HealthSystem variable
            HealthSystem healthSystem;

            //Regen variables
            float regenDelay;
            float regenAmount;
            bool healDecimal;

            bool interrupt;

            bool running;

            //Timer
            float timer;

            void Update()
            {
                if (healthSystem.IsAlive()) HealTimer(); //If we are alive -> regen as normal
            }

            void HealTimer()
            {
                if (!running) return;

                timer += Time.deltaTime; //Increment timer

                if (timer >= regenDelay) //Is timer greater than or equal to regenDelay, if so heal by regenAmount
                {
                    timer -= regenDelay;

                    if (healDecimal) healthSystem.HealDecimal(regenAmount);
                    else healthSystem.Heal(regenAmount);
                }
            }

            void TookDamage()
            {
                timer = 0f;
            }

            public void SetRegenDelay(float delay)
            {
                if (delay == -1) running = false;
                else running = true;

                regenDelay = delay; //Set regen delay
            }

            //Set regen amount
            public void SetRegenAmount(float amount)
            {
                regenAmount = amount; //Set regen amount
            }

            //Setup regen
            public void Setup(HealthSystem healthSystem, bool interrupt, bool healDecimal = false)
            {
                this.healthSystem = healthSystem;
                this.healDecimal = healDecimal;
                this.interrupt = interrupt;

                if (interrupt) healthSystem.OnTookDamage += TookDamage;
            }

            void OnDisable()
            {
                if (interrupt) healthSystem.OnTookDamage -= TookDamage;
            }
        }
    }

    namespace Commands
    {
        public static class CommandSystem
        {
            public static List<DebugCommandBase> commandList = new List<DebugCommandBase>();

            public static void AddCommand(DebugCommandBase command)
            {
                if (command.commandName.Contains(" ")) command.commandName.Replace(" ", "_");
                commandList.Add(command);
            }
        }

        public abstract class DebugCommandBase : IComparable<DebugCommandBase>
        {
            public string commandName;

            public Type[] types;

            public bool hidden;

            public DebugCommandBase(string commandName, bool hidden = false)
            {
                this.commandName = commandName;
                this.hidden = hidden;
            }

            public abstract bool TryExecute(int parameterCount, string[] properties);

            public int CompareTo(DebugCommandBase other)
            {
                return string.Compare(commandName, other.commandName);
            }
        }

        public class DebugCommand : DebugCommandBase
        {
            Action command;

            public DebugCommand(string commandName, Action command, bool hidden = false) : base(commandName, hidden)
            {
                this.command = command;
                types = new Type[0];
            }

            public void Execute()
            {
                command.Invoke();
            }

            public override bool TryExecute(int parametercount, string[] properties)
            {
                command.Invoke();
                return true;
            }
        }

        public class DebugCommand<T1> : DebugCommandBase
        {
            Action<T1> command;

            public DebugCommand(string commandName, Action<T1> command, bool hidden = false) : base(commandName, hidden)
            {
                this.command = command;
                types = new Type[] { typeof(T1) };
            }

            public void Execute(T1 value)
            {
                command.Invoke(value);
            }

            public override bool TryExecute(int parameterCount, string[] properties)
            {
                if (parameterCount != types.Length) return false;

                Execute((T1)Convert.ChangeType(properties[1], typeof(T1)));
                return true;
            }
        }

        public class DebugCommand<T1, T2> : DebugCommandBase
        {
            Action<T1, T2> command;

            public DebugCommand(string commandName, Action<T1, T2> command, bool hidden = false) : base(commandName, hidden)
            {
                this.command = command;
                types = new Type[] { typeof(T1), typeof(T2) };
            }

            public void Execute(T1 value, T2 value2)
            {
                command.Invoke(value, value2);
            }

            public override bool TryExecute(int parameterCount, string[] properties)
            {
                if (parameterCount != types.Length) return false;

                Execute((T1)Convert.ChangeType(properties[1], typeof(T1)), (T2)Convert.ChangeType(properties[2], typeof(T2)));
                return true;
            }
        }
    }
}