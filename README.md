# ⛅ Unity Weather Dashboard

A lightweight, clean-architecture weather dashboard built in Unity using **UI Toolkit** and **Open-Meteo REST API**.

I created this showcase project to demonstrate clean, modular C# design in Unity—specifically applying **SOLID principles** to separate network operations, UI binding, asset mapping, and data formatting into distinct, single-responsibility components.
<img width="1104" height="697" alt="image 43" src="https://github.com/user-attachments/assets/33389149-2065-4e29-a183-8cc38f4478eb" />


---

## 🛠️ Key Features

* **Real-time API Integration:** Asynchronous weather fetching using Open-Meteo's REST API and modern C# `Awaitable`.
* **UI Toolkit (UXML/USS):** Runtime UI constructed with modern UXML layouts and styled via CSS-like stylesheets.
* **Dynamic Forecasts:** Live current conditions, 4-hour slot hourly breakdown, and a 3-day temperature range forecast.
* **Data-Driven Icon Mapping:** Context-aware day/night condition icons mapped via ScriptableObjects without hardcoded logic in UI components.

---

## 🏗️ Architecture & SOLID Design

* **`IWeatherService` / `WeatherService`:** Interface-driven REST API fetching using `UnityWebRequest` and `Awaitable` for easy mock dependency injection.
* **`WeatherDashboardView`:** Dedicated view layer encapsulating UI Toolkit `VisualElement` queries and DOM updates.
* **`WeatherIconDB`:** `ScriptableObject` database acting as a Strategy pattern for mapping WMO weather codes to sprite assets.
* **`WeatherFormatter`:** Pure static utility handling string conversions, ISO date parsing, daytime checks, and cardinal direction math.
* **`WeatherController`:** High-level presenter coordinating data flow between service, database, formatter, and view.

---
