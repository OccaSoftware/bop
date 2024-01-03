# README

## Contents

About
Installation Instructions
Usage Instructions
Requirements
Public API
Support

## About

BOP is a lightweight, easy to use object pooling solution. Object Pooling improves your runtime performance by instantiating objects during scene load rather than during runtime, and by avoiding destroy(object) calls by returning objects back to the pool when no longer needed.

BOP enables you to interact with pooled objects in a similar way to how you would interact with ordinary objects, just more efficiently.

BOP includes a demo that shows how you can connect to the Pool, fetch objects from the pool, dispose of the pool, and create a fresh pool. It also demonstrates how you can get information about the current state of the pool.

## Installation Instructions

1. git clone to your packages folder.

## Usage Instructions

1. Create an empty Game Object
2. Add the Pooler script to this Game Object
3. Drag the prefab you would like to pool into the Object To Pool input
4. Define the number of objects you would like to set up on Awake in the initial count input.
5. Define where you would like these objects to be kept when inactive in the Storage Position input.
6. You can now dynamically fetch objects from the pooler using the built-in public methods, primarily GetFromPool(), detailed further below in the API details.

Note that BOP's default behavior is to dynamically extend the pool as needed by instantiating additional objects. This interaction keeps your project behaving as expected, but can cause you to miss out on the performance benefits of Object Pooling. It is recommended that you set the Initial Count in excess of your expected maximum concurrent object requirements.

Note further that BOP operates in such a way that there is one Pooler for each type of Object. The recommended user behavior is that you create one Pooler for each Object type. You can group Poolers in your hierarchy under a common GameObject for organization. You need a reference to a Pooler in order to use the Public API.

When we create an object pool, we add an Instance component to each object. This Instance component maintains the relationship between the Instance and the Pooler. You should not assign Instance components to game objects manually. You can get information from the Instance component during runtime using public methods, detailed below.

When we fetch an object from the pool, it will be activated and OnEnable will be called. when we return an object to the pool, it will be deactivated and OnDisable will be called. It is recommended that you use these built-in methods to handle any spawn/despawn behaviors that may have been previously handled in Awake, Start, or OnDestroy.

## Public API

The Pooler class includes the following public methods. These can be viewed directly in source in the Pooler.cs file.

### GetFromPool

```cs
public GameObject GetFromPool();
public GameObject GetFromPool(Vector3 position, Quaternion rotation);
public GameObject GetFromPool(Transform parent);
public GameObject GetFromPool(Vector3 position, Quaternion rotation, Transfrom parent);
```

Gets the next unused object from the pool. Can be fetched with a predefined position, rotation, and/or parent.

### IncreasePoolSize

```cs
public int IncreasePoolSize(int count);
```

Increases the size of the pool. Return value is the index of the first new instance. The first new instance is not guaranteed to be the most recent unused object in the pool, so this should be used together with GetFromPool.

### CreateNewPool

```cs
public void CreateNewPool(int count);
```

Disposes of the existing pool and creates an entire fresh new pool of size count. Note that any objects from the pool that are active at the time of this call will be despawned and destroyed, so exercise caution when calling this method while active objects exist.

### DisposePool

```cs
public void DisposePool();
```

Disposes of the existing pool. Note that any objects from the pool that are active at the time of this call will be despawned and destroyed, so exercise caution when calling this method while active objects exist.

### GetPoolStats()

```cs
public PoolStatistics GetPoolStats();
```

Returns the Pool's size, active count, and inactive count. It is recommended that you do not call this method every frame as it can be expensive.

The Instance class is dynamically added to new object instances when they are created during the Pooler spinup process. It includes the following public methods. These can be viewed directly in source in the Instance.cs file.

### GetPoolerOrigin

```cs
public Pooler GetPoolerOrigin();
```

Returns the owning pooler.

### GetIndex

```cs
public int GetIndex();
```

Returns the Instance index, used by the pooler for tracking.

### Despawn

```cs
public void Despawn();
```

Causes the object to despawn and be returned to the origin pool. It is recommended that you call this method instead of Destroy.
