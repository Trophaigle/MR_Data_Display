# 📡 MR_Data_Display

## 🌐 Immersive XR Weather Visualization System

MR_Data_Display is an immersive XR application that visualizes weather forecast data directly inside the user’s physical environment.

Built for **Meta Quest 3**, the system combines **Unity XR**, **FastAPI**, and **WebSockets** to stream real-time weather data and represent it as interactive spatial visuals.

---

# 🎯 Project Overview

This project transforms traditional weather forecasts into an **interactive 3D spatial experience**:

- 📊 3D histogram representing daily weather data
- 🌡️ Real-time temperature visualization per day
- 📈 KPI panels for key weather metrics
- 🔄 Automatic updates every 15 seconds
- 🧭 Fully interactive XR environment (grab, move, explore)

Instead of reading weather data, the user **experiences it in space**.

---

# 🧠 Design Philosophy

The application is designed around **spatial intuition and real-time data immersion**:

- Weather is visualized as **3D objects in space**
- Each day is represented as a **movable histogram bar**
- Users can physically interact with the dataset in XR
- KPI panels provide quick analytical insights
- Visual effects enhance perception of temperature changes

The goal is to turn abstract numerical data into a **tangible environmental experience**.

---

# 🧰 Tech Stack

## Backend
- Python
- FastAPI
- WebSockets
- Uvicorn
- OpenWeatherMap API

## Frontend (XR Client)
- Unity
- C#
- Unity XR Toolkit
- Meta Quest 3 SDK
- WebSocket communication
- Visual Effects Graph / Particle Systems

---

# 📦 Project Structure

```text
MR_Data_Display/
│
├── backend_python/        # FastAPI server (weather + websocket)
├── unity_client-dataMR/   # Unity XR application
├── files/                 # Media assets (ignored by git)
└── README.md
```

# 🚀 Getting Started

## 1. Backend Setup (FastAPI)

### Install dependencies

```bash
cd backend_python
pip install -r requirements.txt
```

### Run the server
```bash
uvicorn main:app --host 0.0.0.0 --port 8000 --reload
```

## 2. Find your local IP address
```bash
ipconfig
```
Look for
```bash
IPv4 Address . . . . . . . . . : 192.168.1.xxx
```
## 3. Configure Unity connection
In Unity, fill Ip field with IPv4

## 4. Run Unity XR App
- Open unity_client-dataMR in Unity Hub
- Switch platform to Android (Meta Quest 3)
- Build & Run or use Link

# 🔌 Communication System
Unity XR Client ← WebSocket → FastAPI Backend ← OpenWeatherMap API

- REST API: initial data fetch
- WebSocket: real-time updates every 15 seconds

# 📊 Features
## 🌤 Weather Visualization
- 3D histogram for daily forecasts
- Temperature mapped to height
- Color-coded weather states

## 🧭 XR Interaction
- Grab and move histogram in space
- Spatial KPI panels
- Immersive exploration

## 🔄 Real-time updates
- Data refreshed every 15 seconds
- Live synchronization with backend

## 🎮 Meta Quest 3 Support
- Fully immersive XR experience
- Hand tracking

# 🧩 Key Technical Choices
- WebSockets for low-latency real-time updates
- FastAPI for lightweight async backend
- Unity XR Toolkit for cross-device XR interaction
- Spatial UI instead of flat HUD
- Modular architecture (backend/client separation)
- OpenWeatherMap API for live weather data
