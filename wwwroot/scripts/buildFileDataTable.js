document.addEventListener('DOMContentLoaded', async () => {
    let elementTypes = await fetchElementTypes();
    let summaryData = await fetchSummaryData();

    await fetchData(elementTypes, summaryData);
});

async function fetchData(elementTypes, summaryData) {
    try {
        fileId = getFileNameFromURL();
        const response = await fetch(`http://localhost:5000/BankData/${fileId}`);
        if (!response.ok) {
            throw new Error();
        }
        const data = await response.json();
        createTable(data["accounts"], elementTypes.elementTypes, summaryData);
    } catch (error) {
        console.error('Ошибка при получении данных:', error.message);
    }
}

async function fetchElementTypes() {
    try {
        const response = await fetch(`http://localhost:5000/BankData/element_types`);
        if (!response.ok) {
            throw new Error();
        }
        return await response.json();
    } catch (error) {
        console.error('Ошибка при получении данных:');
    }
}

async function fetchSummaryData(){
    try {
        fileId = getFileNameFromURL();
        const response = await fetch(`http://localhost:5000/BankData/${fileId}/summary`);
        if (!response.ok) {
            throw new Error();
        }
        return await response.json();
    } catch (error) {
        console.error('Ошибка при получении данных:', error);
    }
}

function createTable(data, elementTypes, summaryData) {
    console.log(elementTypes)
    const groupedData = groupDataByClass(data);
    const container = document.getElementById('data-container');
    for (const classCode in groupedData) {
        const classItems = groupedData[classCode];
        const classDiv = document.createElement('div');
        classDiv.innerHTML = `<h2>${classCode} ${classItems[0].className}</h2>`;
        const table = document.createElement('table');
        const headerRow = createHeaderRow(elementTypes);
        table.appendChild(headerRow);

        classItems.forEach(account => {
            const row = createDataRow(account, elementTypes);
            table.appendChild(row);
        });

        const summaryRow = createSummaryRow(summaryData.classElements[classCode], elementTypes);
        table.appendChild(summaryRow);

        classDiv.appendChild(table);
        container.appendChild(classDiv);
    }
}

function groupDataByClass(data) {
    return data.reduce((acc, item) => {
        const classCode = item.classCode;
        if (!acc[classCode]) {
            acc[classCode] = [];
        }
        acc[classCode].push(item);
        return acc;
    }, {});
}

function createSummaryRow(classSummary, elementTypes) {
    const row = document.createElement('tr');
    const accountCodeCell = document.createElement('td');
    accountCodeCell.innerHTML = "<strong>По классу</strong>";
    row.appendChild(accountCodeCell);

    const valuesMap = {};
    classSummary.forEach(element => {
        valuesMap[element.elementTypeId] = element.value;
    });

    for (const typeId in elementTypes) {
        const td = document.createElement('td');
        td.innerHTML = valuesMap[typeId] !== undefined ? `<strong>${valuesMap[typeId]}</strong>` : "none";
        row.appendChild(td);
    }

    return row;
}

function createHeaderRow(elementTypes) {
    const headerRow = document.createElement('tr');
    const accountCodeHeader = document.createElement('th');
    accountCodeHeader.innerText = "Код аккаунта";
    headerRow.appendChild(accountCodeHeader);

    for (const typeId in elementTypes) {
        const th = document.createElement('th');
        th.innerText = elementTypes[typeId];
        headerRow.appendChild(th);
    }
    return headerRow;
}

function createDataRow(account, elementTypes) {
    const row = document.createElement('tr');
    const accountCodeCell = document.createElement('td');
    accountCodeCell.innerText = account.accountCode;
    row.appendChild(accountCodeCell);

    const valuesMap = {};
    account.elements.forEach(element => {
        valuesMap[element.elementtypeid] = element.value;
    });

    for (const typeId in elementTypes) {
        const td = document.createElement('td');
        td.innerText = valuesMap[typeId] !== undefined ? valuesMap[typeId] : "none";
        row.appendChild(td);
    }

    return row;
}
function getFileNameFromURL() {
    const params = new URLSearchParams(window.location.search);
    return params.get('fileid');
}