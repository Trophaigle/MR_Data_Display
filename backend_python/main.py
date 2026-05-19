from fastapi import FastAPI, WebSocket
import asyncio
import requests

app = FastAPI()

def weather_to_effect(code):

    # ☀️ Clear sky
    if code == 0:
        return "sunny"

    # 🌤️ Mainly clear / partly cloudy (still sunny-ish)
    elif code in [1, 2, 3]:
        return "sunny"

    # ☁️ Cloudy / overcast / fog
    elif code in [45, 48]:
        return "cloudy"

    # 🌧 Rain / drizzle
    elif 51 <= code <= 67 or 80 <= code <= 82:
        return "rain"

    # ⛈ Thunderstorm
    elif 95 <= code <= 99:
        return "storm"

    return "cloudy"


def fetch_kpi():
    url = "https://api.open-meteo.com/v1/forecast"

    params = {
        "latitude": 44.84,
        "longitude": -0.58,
        "daily": [
            "temperature_2m_max",
            "weathercode"
        ],
        "timezone": "auto",
        "forecast_days": 14
    }

    data = requests.get(url, params=params).json()

    weathercodes = data["daily"]["weathercode"]

    # 🔥 Conversion codes → effets MR
    effects = [weather_to_effect(code) for code in weathercodes]

    return {
        "days": data["daily"]["time"],
        "values": data["daily"]["temperature_2m_max"],
        "effects": effects,
        "avg_temp": (
            sum(data["daily"]["temperature_2m_max"])
            / len(data["daily"]["temperature_2m_max"])
        )
    }


@app.websocket("/ws/kpi")
async def kpi_socket(websocket: WebSocket):
    await websocket.accept()

    while True:
        kpi = fetch_kpi()

        await websocket.send_json(kpi)

        # update toutes les 5 secondes
        await asyncio.sleep(5)