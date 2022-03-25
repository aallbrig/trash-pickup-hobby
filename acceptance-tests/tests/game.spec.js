const assert = require("assert");
const puppeteer = require("puppeteer");
const HEADLESS_MODE = process.env.HEADLESS_MODE || false;
const GAME_URL = process.env.GAME_URL || "http://localhost:8000/WebGL/index.html";

let browser;
let page;

describe("A page for a fun video game", () => {
  before(async () => {
    browser = await puppeteer.launch({
      headless: HEADLESS_MODE,
      args: ['--disable-dev-shm-usage']
    });
    page = await browser.newPage();
  });
  after(async () => {
    await browser.close();
  });

  it("Should exist", async () => {
    await page.goto(GAME_URL);
    let title = await page.title();
    assert.equal(title, "Unity WebGL Player | Trash Pickup Video Game");
  });
});
