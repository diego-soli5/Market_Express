async function delayAsync(time) {
    await new Promise(r => setTimeout(r, time));
}