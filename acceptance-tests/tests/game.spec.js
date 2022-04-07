const assert = require("assert");
const puppeteer = require("puppeteer");
const HEADLESS_MODE = process.env.HEADLESS_MODE || false;
const GAME_URL = process.env.GAME_URL || "http://localhost:8000/WebGL/index.html";

describe("A webpage for a fun trash pickup inspired video game", () => {
  let browser;
  before(async () => {
    browser = await puppeteer.launch({
      headless: HEADLESS_MODE,
      args: ['--disable-dev-shm-usage']
    });
  });
  after(async () => {
    await browser.close();
  });

  it("Should have a 🔥 page title", async () => {
    const page = await browser.newPage();
    await page.goto(GAME_URL);
    let title = await page.title();
    assert.equal(title, "Unity WebGL Player | Trash Pickup Video Game");
  });

  it("Should contain a unity webGL video game 🕹", async () => {
    const page = await browser.newPage();
    await page.goto(GAME_URL);
    // Find the HTML representation of the unity player
    const unityPlayer = await page.$('#unity-container canvas#unity-canvas');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(unityPlayer, null);
  });
});
