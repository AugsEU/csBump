# csBump

csBump is a port of the Java library, [jBump](https://github.com/implicit-invocation/jbump), written for C#.

### What is csBump?

This is a library for simple AABB collisions. It does not support any other kind of collider. A world is created then rectangles can be added to that world.

This is still in development but it is functional. Until the API has been solified there is no nuget package.

### Setup from source

Clone the repository and then the csproj to your solution.

For MonoGame projects, add the "csBump.MG" project, else add the normal "csBump" project.

### Usage

#### 1. Create your world

A world is simply a space in which things can interact. Objects within the world will collide, and objects in separate worlds cannot interact.

```cs
World bumpWorld = new World();
```

#### 2. Add elements to the world

All elements in the world are simple rectangles. When adding them to our world, we need to use a "BumpID", which is just an ID we can use to later query the world.

```cs
BumpID mBumpItem = new BumpID();
bumpWorld.Add(mBumpItem, mPosition.X, mPosition.Y, mSize.X, mSize.Y);
```

#### 3. Move our items

Now, using our BumpID, we can ask the world to move our rectangle. It will handle all collisions for us and give us the result. In order for this movement to apply to our game objects we can then get the rectangles new position and apply that to our game objects.

```cs
CollisionResult result = bumpWorld.Move(mBumpItem, desirePos, new DefaultFilter());

Rect2f rect = bumpWorld.GetRect(mBumpItem);
mPosition.X = rect.X;
mPosition.Y = rect.Y;
```