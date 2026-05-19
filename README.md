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

# 🚀 Getting Started

## 1. Backend Setup (FastAPI)

### Install dependencies

```bash
cd backend_python
pip install -r requirements.txt
